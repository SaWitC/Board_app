using Board.Application.Abstractions.Repositories;
using Board.Application.DTOs;
using FastEndpoints;

namespace Board.Api.Features.BoardColumn.CreateBoardColumn;

public class CreateBoardColumnEndpoint : Endpoint<CreateBoardItemRequest>
{
    private readonly IBoardColumnRepository _columnRepository;
    private readonly IBoardRepository _boardRepository;

    public CreateBoardColumnEndpoint(IBoardColumnRepository columnRepository, IBoardRepository boardRepository)
    {
        _columnRepository = columnRepository;
        _boardRepository = boardRepository;
    }
    public override void Configure()
    {
        Post("/api/boards/{boardId}/columns");
    }

    public override async Task HandleAsync(CreateBoardItemRequest request, CancellationToken cancellationToken)
    {
        Guid boardId = Route<Guid>("boardId");
        Domain.Entities.Board board = await _boardRepository.GetAsync(b => b.Id == boardId, cancellationToken, true, b => b.BoardColumns);
        if (board == null)
        {
            throw new InvalidOperationException("Board not found");
        }

        Domain.Entities.BoardColumn entity = new Domain.Entities.BoardColumn
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description
        };

        board.BoardColumns.Add(entity);
        await _columnRepository.AddAsync(entity, cancellationToken);
        await _boardRepository.UpdateAsync(board, cancellationToken);

        BoardColumnDto response = new BoardColumnDto
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description
        };

        await Send.OkAsync(response, cancellationToken);
    }
}
