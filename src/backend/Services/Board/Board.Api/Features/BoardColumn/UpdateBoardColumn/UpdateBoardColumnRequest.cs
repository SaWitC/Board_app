namespace Board.Api.Features.BoardColumn.UpdateBoardColumn;

public class UpdateBoardColumnRequest
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
}
