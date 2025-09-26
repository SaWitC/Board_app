namespace Board.Application.DTOs.Tags;

public class TagDto
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public string Description { get; set; }
}
