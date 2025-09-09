using Board.Application.DTOs;
using Board.Application.Interfaces;
using FastEndpoints;

namespace Board.Api.Features.BoardColumn.GetBoardColumns;

public class GetBoardColumnsEndpoint : EndpointWithoutRequest
{
    private readonly IRepository<Domain.Entities.Board> _boardRepository;

    public GetBoardColumnsEndpoint(IRepository<Domain.Entities.Board> boardRepository)
    {
        _boardRepository = boardRepository;
    }

    public override void Configure()
    {
        Get("/api/boards/{boardId}/columns");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        Guid boardId = Route<Guid>("boardId");

        Domain.Entities.Board board = await _boardRepository.GetAsync(b => b.Id == boardId, cancellationToken, true, b => b.BoardColumns);
        if (board == null)
        {
            await Send.OkAsync(Array.Empty<BoardColumnDto>(), cancellationToken);
        }

        List<BoardColumnDto> response = board.BoardColumns
            .Select(c => new BoardColumnDto
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description
            })
            .ToList();

        await Send.OkAsync(response, cancellationToken);
    }
}
