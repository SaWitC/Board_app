namespace Board.Api.Features.Board.GetBoards;

public class GetBoardsRequest
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 12;
    public string TitleSearchTerm { get; set; }
    public string OwnerSearchTerm { get; set; }
}
