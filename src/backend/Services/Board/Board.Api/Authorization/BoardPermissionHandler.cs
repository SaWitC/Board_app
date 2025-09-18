using Board.Application.Abstractions.Repositories;
using Board.Application.Abstractions.Services;
using Board.Application.Security;
using Board.Domain.Contracts.Security;
using Board.Domain.Entities;
using Board.Domain.Security;
using Microsoft.AspNetCore.Authorization;

namespace Board.Api.Authorization;

public sealed class BoardPermissionHandler : AuthorizationHandler<BoardPermissionRequirement>
{
    private readonly IBoardRepository _boardRepository;
    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public BoardPermissionHandler(
        IBoardRepository boardRepository,
        ICurrentUserProvider currentUserProvider,
        IHttpContextAccessor httpContextAccessor)
    {
        _boardRepository = boardRepository;
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
        string email = _currentUserProvider.GetUserEmail();
        if (string.IsNullOrWhiteSpace(email))
        {
            return;
        }

        // Global permissions via 'permissions' claims
        HashSet<string> perms = httpContext.User?.Claims
            ?.Where(c => string.Equals(c.Type, Auth.Claims.Permissions, StringComparison.OrdinalIgnoreCase))
            .Select(c => c.Value)
            .ToHashSet(StringComparer.OrdinalIgnoreCase) ?? new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        if (perms.Contains(Auth.Roles.GlobalAdmin))
        {
            context.Succeed(requirement);
        }

        TryGetRouteGuid(httpContext, requirement.RouteKey, out Guid boardId);

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
}
