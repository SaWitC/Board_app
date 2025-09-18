using Board.Domain.Contracts.Enums;
using Board.Domain.Contracts.Security;

namespace Board.Application.Security;

public static class RolePermissionsMapper
{
    public static Permission GetPermissions(UserAccessEnum role)
    {
        return role switch
        {
            UserAccessEnum.BoardUser => Permission.Read | Permission.None | Permission.ManageItems,
            UserAccessEnum.BoardMGR => Permission.Read | Permission.None | Permission.ManageItems | Permission.ManageColumns | Permission.ManageUsers,
            UserAccessEnum.BoardOwner => Permission.Read | Permission.None | Permission.ManageItems | Permission.ManageColumns | Permission.ManageUsers | Permission.ManageBoard,

            //UserAccessEnum.User => Permission.Read,
            UserAccessEnum.GlobalAdmin => Permission.Read | Permission.None | Permission.ManageItems | Permission.ManageColumns | Permission.ManageUsers | Permission.ManageBoard,
            _ => Permission.None
        };
    }
}
