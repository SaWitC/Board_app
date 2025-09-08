using Board.Application.Interfaces;
using FastEndpoints;

namespace Board.Api.Features.Board.DeleteBoard;

public class DeleteBoardEndpoint : EndpointWithoutRequest
{
    public override void Configure()
    {
        Delete("/api/boards/{id}");
        AllowAnonymous();
    }

    private readonly IRepository<Domain.Entities.Board> _repository;

    public DeleteBoardEndpoint(IRepository<Domain.Entities.Board> repository)
    {
        _repository = repository;
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        Guid id = Route<Guid>("id");

        Domain.Entities.Board entity = await _repository.GetAsync(x => x.Id == id, cancellationToken);
        if (entity == null)
        {
            await Send.OkAsync(false, cancellationToken);
        }

        await _repository.DeleteAsync(entity, cancellationToken);

        await Send.OkAsync(true, cancellationToken);
    }
}


