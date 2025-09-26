namespace Board.Application.DTOs;

public class BoardDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public List<BoardUserDto> BoardUsers { get; set; } = [];
    public DateTimeOffset ModificationDate { get; set; }
    public bool IsTemplate { get; set; }
    public bool IsActiveTemplate { get; set; }
}
