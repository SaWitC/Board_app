namespace Board.Domain.Contracts.Enums;

public enum UserAccessEnum
{
	// Per-board roles
	BoardUser = 7, 
	BoardMGR = 127,
	BoardOwner = 1023,

	// Global roles (not typically stored per-board; used via claims)
	User = 1, // read-only global user
	GlobalAdmin = 65535 // full platform admin
}
