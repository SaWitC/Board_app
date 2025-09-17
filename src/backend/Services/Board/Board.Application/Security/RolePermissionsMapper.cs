using Board.Domain.Contracts.Enums;
using Board.Domain.Contracts.Security;

namespace Board.Application.Security;

public static class RolePermissionsMapper
{
    public static Permission GetPermissions(UserAccessEnum role)
    {
        return role switch
        {
            UserAccessEnum.BoardUser => Permission.Read | Permission.Create | Permission.Edit | Permission.Delete,
			UserAccessEnum.BoardMGR => Permission.Read | Permission.Create | Permission.Edit | Permission.Delete | Permission.ManageUsers,
			UserAccessEnum.BoardOwner => Permission.Read | Permission.Create | Permission.Edit | Permission.Delete | Permission.ManageUsers | Permission.DeleteBoard,
			
            UserAccessEnum.User => Permission.Read,
			UserAccessEnum.GlobalAdmin => Permission.Read | Permission.Create | Permission.Edit | Permission.Delete | Permission.ManageUsers | Permission.DeleteBoard,
            _ => Permission.None
        };
    }
}
