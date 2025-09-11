namespace Board.Application.DTOs;

public class BoardTemplateDto
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public Guid BoardId { get; set; }
}


