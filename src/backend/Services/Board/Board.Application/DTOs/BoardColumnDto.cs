namespace Board.Application.DTOs;

public class BoardColumnDto
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public int Order { get; set; }
}
