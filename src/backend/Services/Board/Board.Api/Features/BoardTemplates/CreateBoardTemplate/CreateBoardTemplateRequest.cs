namespace Board.Api.Features.BoardTemplates.CreateBoardTemplate;

public class CreateBoardTemplateRequest
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public Guid BoardId { get; set; }
}


