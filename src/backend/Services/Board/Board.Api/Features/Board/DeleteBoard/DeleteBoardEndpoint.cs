using Board.Application.Abstractions.Repositories;
using FastEndpoints;

namespace Board.Api.Features.Board.DeleteBoard;

public class DeleteBoardEndpoint : EndpointWithoutRequest
{
    public override void Configure()
    {
        Delete("/api/boards/{id}");
    }

    private readonly IBoardRepository _repository;
    public DeleteBoardEndpoint(IBoardRepository repository)
    {
        _repository = repository;
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        Guid id = Route<Guid>("id");

        Domain.Entities.Board entity = await _repository.GetAsync(x => x.Id == id, cancellationToken);

        await _repository.DeleteAsync(entity, cancellationToken);

        await Send.OkAsync(true, cancellationToken);
    }
}


