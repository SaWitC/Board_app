using Board.Domain.Contracts.Enums;
using Board.Domain.Contracts.Security;
using Microsoft.AspNetCore.Authorization;

namespace Board.Api.Authorization;

public sealed class BoardPermissionRequirement : IAuthorizationRequirement
{
	public BoardPermissionRequirement(Permission required, Context context, string routeKey)
	{
		Required = required;
		Context = context;
		RouteKey = routeKey;
	}

	public Permission Required { get; }
	public Context Context { get; }
	public string RouteKey { get; }
} 
