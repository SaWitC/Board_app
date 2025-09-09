namespace Board.Domain.Entities;
public class BoardUsers
{
    public Guid BoardId { get; set; }
    public Guid UserId { get; set; }
    public UserAccessEnum Role { get; set; }
}


public enum UserAccessEnum
{
    User = 7,//bits 0000 0111
    Admin = 127,//bits 111 111 1
    Owner = 1023//bits 111 111 111 1
}

