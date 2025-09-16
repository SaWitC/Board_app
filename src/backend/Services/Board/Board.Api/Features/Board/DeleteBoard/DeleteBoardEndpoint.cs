using Board.Application.Abstractions.Repositories;
using Board.Application.Abstractions.Services;
using FastEndpoints;

namespace Board.Api.Features.Board.DeleteBoard;

public class DeleteBoardEndpoint : EndpointWithoutRequest
{
    public override void Configure()
    {
        Delete("/api/boards/{id}");
    }

    private readonly IBoardRepository _repository;
    private readonly ICurrentUserProvider _currentUserProvider;
    public DeleteBoardEndpoint(IBoardRepository repository, ICurrentUserProvider currentUserProvider)
    {
        _repository = repository;
        _currentUserProvider = currentUserProvider;
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        Guid id = Route<Guid>("id");

        Domain.Entities.Board entity = await _repository.GetAsync(x => x.Id == id, cancellationToken);

        Domain.Entities.BoardUser boardOwner = await _repository.GetBoardOwnerAsync(entity.Id, cancellationToken);
        if (boardOwner.Email != _currentUserProvider.GetCurrentUserEmail())
        {
            throw new Exception("You can not remove this board");
        }

        await _repository.DeleteAsync(entity, cancellationToken);

        await Send.OkAsync(true, cancellationToken);
    }
}


