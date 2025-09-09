using Board.Domain.Contracts.Enums;

namespace Board.Domain.Entities;

public class BoardUser
{
    public string Email { get; set; }
    public Guid BoardId { get; set; }
    public UserAccessEnum Role { get; set; }
}
