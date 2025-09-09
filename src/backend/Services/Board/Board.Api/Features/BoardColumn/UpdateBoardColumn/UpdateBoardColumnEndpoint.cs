using Board.Application.DTOs;
using Board.Application.Interfaces;
using FastEndpoints;

namespace Board.Api.Features.BoardColumn.UpdateBoardColumn;

public class UpdateBoardColumnEndpoint : Endpoint<UpdateBoardColumnRequest>
{
    private readonly IRepository<Domain.Entities.BoardColumn> _repository;

    public UpdateBoardColumnEndpoint(IRepository<Domain.Entities.BoardColumn> repository)
    {
        _repository = repository;
    }

    public override void Configure()
    {
        Put("/api/boards/{boardId}/columns");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateBoardColumnRequest request, CancellationToken cancellationToken)
    {
        Domain.Entities.BoardColumn entity = await _repository.GetAsync(x => x.Id == request.Id, cancellationToken, false);
        if (entity == null)
        {
            throw new InvalidOperationException("Column not found");
        }

        entity.Title = request.Title;
        entity.Description = request.Description;

        Domain.Entities.BoardColumn updated = await _repository.UpdateAsync(entity, cancellationToken);

        BoardColumnDto response = new BoardColumnDto
        {
            Id = updated.Id,
            Title = updated.Title,
            Description = updated.Description
        };
        await Send.OkAsync(response, cancellationToken);
    }
}
