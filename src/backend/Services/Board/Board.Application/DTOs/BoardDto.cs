namespace Board.Application.DTOs;

public class BoardDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public List<string> Users { get; set; } = [];
    public List<string> Admins { get; set; } = [];
    public List<string> Owners { get; set; } = [];
    public List<BoardColumnDto> BoardColumns { get; set; } = [];
    public DateTimeOffset ModificationDate { get; set; }
}
