using Board.Application.Abstractions.Repositories;
using FastEndpoints;

namespace Board.Api.Features.BoardColumn.DeleteBoardColumn;

public class DeleteBoardColumnEndpoint : EndpointWithoutRequest
{
    private readonly IBoardColumnRepository _repository;

    public DeleteBoardColumnEndpoint(IBoardColumnRepository repository)
    {
        _repository = repository;
    }
    public override void Configure()
    {
        Delete("/api/boards/{boardId}/columns/{id}");
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        Guid id = Route<Guid>("id");
        Domain.Entities.BoardColumn entity = await _repository.GetAsync(x => x.Id == id, cancellationToken, false);
        if (entity == null)
        {
            await Send.OkAsync(false, cancellationToken);
        }

        await _repository.DeleteAsync(entity, cancellationToken);
        await Send.OkAsync(true, cancellationToken);

    }
}
