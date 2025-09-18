namespace Board.Domain.Contracts.Security;

[Flags]
public enum Permission
{
    None = 0,
    Read = 1,//Read board and its content
    ManageItems = 2,
    ManageColumns = 4,
    ManageUsers = 8,
    ManageBoard = 16
}
