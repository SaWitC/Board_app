namespace Board.Domain.Contracts.Security;

[Flags]
public enum Permission
{
	None = 0,
	Read = 1,
	Create = 2,
	Edit = 4,
	Delete = 8,
	ManageUsers = 16,
	DeleteBoard = 32
} 
