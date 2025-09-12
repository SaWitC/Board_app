using Board.Application.Interfaces;
using Board.Domain.Entities;
using FastEndpoints;

namespace Board.Api.Features.BoardItems.DeleteBoardItem;

public class DeleteBoardItemEndpoint : EndpointWithoutRequest
{
    private readonly IRepository<BoardItem> _repository;

    public DeleteBoardItemEndpoint(IRepository<BoardItem> repository)
    {
        _repository = repository;
    }
    public override void Configure()
    {
        Delete("/api/boarditems/{id}");
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        Guid id = Route<Guid>("id");

        BoardItem entity = await _repository.GetAsync(x => x.Id == id, cancellationToken);
        if (entity == null)
        {
            await Send.OkAsync(false, cancellationToken);
        }

        await _repository.DeleteAsync(entity, cancellationToken);
        await Send.OkAsync(true, cancellationToken);
    }
}


