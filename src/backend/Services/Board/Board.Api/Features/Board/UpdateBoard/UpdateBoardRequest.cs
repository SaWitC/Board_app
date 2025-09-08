namespace Board.Api.Features.Board.UpdateBoard;

public class UpdateBoardRequest
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
}
