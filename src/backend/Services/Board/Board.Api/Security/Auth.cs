using Board.Domain.Contracts.Enums;
using Board.Domain.Contracts.Security;

namespace Board.Api.Security;

public static class Auth
{
    public static class Claims
    {
        public const string Permissions = "permissions";
    }

    public static class Roles
	{
		public const string GlobalAdmin = "GlobalAdmin";
	}

	public static class Policies
	{
		public const string AuthenticatedUser = "AuthenticatedUser";
	}

	public static string BuildPermissionPolicy(Permission permission, Context context, string routeKey)
		=> $"{Claims.Permissions}:{permission}:{context}:{routeKey}";
} 
