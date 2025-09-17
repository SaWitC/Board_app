using System.Text.Json;
using Board.Api.Security;
using Board.Application.Abstractions.Repositories;
using Board.Application.Abstractions.Services;
using Board.Application.Security;
using Board.Domain.Contracts.Enums;
using Board.Domain.Contracts.Security;
using Board.Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace Board.Api.Authorization;

public sealed class BoardPermissionHandler : AuthorizationHandler<BoardPermissionRequirement>
{
	private readonly IBoardRepository _boardRepository;
	private readonly IBoardColumnRepository _boardColumnRepository;
	private readonly IBoardItemRepository _boardItemRepository;
	private readonly ICurrentUserProvider _currentUserProvider;
	private readonly IHttpContextAccessor _httpContextAccessor;

	public BoardPermissionHandler(
		IBoardRepository boardRepository,
		IBoardColumnRepository boardColumnRepository,
		IBoardItemRepository boardItemRepository,
		ICurrentUserProvider currentUserProvider,
		IHttpContextAccessor httpContextAccessor)
	{
		_boardRepository = boardRepository;
		_boardColumnRepository = boardColumnRepository;
		_boardItemRepository = boardItemRepository;
		_currentUserProvider = currentUserProvider;
		_httpContextAccessor = httpContextAccessor;
	}

	protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, BoardPermissionRequirement requirement)
	{
		HttpContext httpContext = _httpContextAccessor.HttpContext;
		if (httpContext == null)
		{
			return;
		}

		CancellationToken ct = httpContext.RequestAborted;
		string email = _currentUserProvider.GetCurrentUserEmail();
		if (string.IsNullOrWhiteSpace(email))
		{
			return;
		}

		// Global permissions via 'permissions' claims
		HashSet<string> perms = httpContext.User?.Claims
			?.Where(c => string.Equals(c.Type, Auth.Claims.Permissions, StringComparison.OrdinalIgnoreCase))
			.Select(c => c.Value)
			.ToHashSet(StringComparer.OrdinalIgnoreCase) ?? new HashSet<string>(StringComparer.OrdinalIgnoreCase);

		if (!perms.Contains(Auth.Roles.GlobalAdmin))
		{
			return;
		}

		Guid boardId;
		switch (requirement.Context)
		{
			case Context.Board:
				// routeKey may present in route or in body
				if (!TryGetRouteGuid(httpContext, requirement.RouteKey, out boardId))
				{
					if (!TryGetGuidFromBody(httpContext, requirement.RouteKey, out boardId))
					{
						return;
					}
				}

				break;
			case Context.BoardColumn:
				// routeKey may present in route or in body
				if (!TryGetRouteGuid(httpContext, requirement.RouteKey, out Guid columnId))
				{
					if (!TryGetGuidFromBody(httpContext, requirement.RouteKey, out columnId))
					{
						return;
					}
				}

				BoardColumn column = await _boardColumnRepository.GetAsync(c => c.Id == columnId, ct, true);
				if (column == null)
				{
					return;
				}

				boardId = column.BoardId;
				break;
			case Context.BoardItem:
				if (!TryGetRouteGuid(httpContext, requirement.RouteKey, out Guid itemId))
				{
					if (!TryGetGuidFromBody(httpContext, requirement.RouteKey, out itemId))
					{
						return;
					}
				}

				BoardItem item = await _boardItemRepository.GetAsync(i => i.Id == itemId, ct, true, i => i.BoardColumn);
				if (item?.BoardColumn == null)
				{
					return;
				}

				boardId = item.BoardColumn.BoardId;
				break;
			default:
				return;
		}

		BoardUser membership = await _boardRepository.GetAsync(
			predicate: b => b.Id == boardId,
			selector: b => b.BoardUsers.FirstOrDefault(u => u.Email == email),
			cancellationToken: ct,
			includes: b => b.BoardUsers);

		if (membership == null)
		{
			return;
		}

		Permission userPermissions = RolePermissionsMapper.GetPermissions(membership.Role);
		if ((userPermissions & requirement.Required) == requirement.Required)
		{
			context.Succeed(requirement);
		}
	}

	private static bool TryGetRouteGuid(HttpContext context, string key, out Guid id)
	{
		id = Guid.Empty;
		if (!context.Request.RouteValues.TryGetValue(key, out object routeObj) || routeObj == null)
		{
			return false;
		}
		string raw = routeObj.ToString();
		return Guid.TryParse(raw, out id);
	}

	private static bool TryGetGuidFromBody(HttpContext context, string propertyName, out Guid id)
	{
		id = Guid.Empty;
		if (context.Request?.Body == null || !context.Request.ContentType?.Contains("application/json", StringComparison.OrdinalIgnoreCase) == true)
		{
			return false;
		}
		try
		{
			context.Request.EnableBuffering();
			using StreamReader reader = new(context.Request.Body, leaveOpen: true);
			string body = reader.ReadToEnd();
			context.Request.Body.Position = 0;
			if (string.IsNullOrWhiteSpace(body))
			{
				return false;
			}

			using JsonDocument doc = JsonDocument.Parse(body);
			if (doc.RootElement.ValueKind != JsonValueKind.Object)
			{
				return false;
			}

			foreach (JsonProperty prop in doc.RootElement.EnumerateObject())
			{
				if (string.Equals(prop.Name, propertyName, StringComparison.OrdinalIgnoreCase))
				{
					string raw = prop.Value.GetString();
					return Guid.TryParse(raw, out id);
				}
			}
			return false;
		}
		catch
		{
			return false;
		}
	}
} 
