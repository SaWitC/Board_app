using FastEndpoints;

namespace Board.Api.Features.BoardTemplates.SearchBoardTemplates;

public class SearchBoardTemplatesRequest
{
    [QueryParam]
    public string SearchTerm { get; set; }
}
