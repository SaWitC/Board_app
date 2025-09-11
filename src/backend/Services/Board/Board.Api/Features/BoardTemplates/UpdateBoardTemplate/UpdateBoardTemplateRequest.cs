namespace Board.Api.Features.BoardTemplates.UpdateBoardTemplate;

public class UpdateBoardTemplateRequest
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public Guid BoardId { get; set; }
    public bool IsActive { get; set; }
}


