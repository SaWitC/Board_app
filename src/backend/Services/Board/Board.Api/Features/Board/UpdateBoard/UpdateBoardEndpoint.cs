using Board.Application.DTOs;
using Board.Application.Interfaces;
using FastEndpoints;

namespace Board.Api.Features.Board.UpdateBoard;

public class UpdateBoardEndpoint : Endpoint<UpdateBoardRequest>
{
    private readonly IRepository<Domain.Entities.Board> _repository;
    public UpdateBoardEndpoint(IRepository<Domain.Entities.Board> repository)
    {
        _repository = repository;
    }
    public override void Configure()
    {
        Put("/api/boards/{id}");
        AllowAnonymous();
    }


    public override async Task HandleAsync(UpdateBoardRequest request, CancellationToken cancellationToken)
    {
        Guid id = Route<Guid>("id");

        Domain.Entities.Board entity = await _repository.GetAsync(x => x.Id == id, cancellationToken);
        if (entity == null)
        {
            await Send.OkAsync(null, cancellationToken);
        }

        entity.Title = request.Title;
        entity.Description = request.Description;
        entity.ModificationDate = DateTimeOffset.UtcNow;

        Domain.Entities.Board updated = await _repository.UpdateAsync(entity, cancellationToken);
        BoardDto response = new BoardDto
        {
            Id = updated.Id,
            Title = updated.Title,
            Description = updated.Description,
            ModificationDate = updated.ModificationDate
        };

        await Send.OkAsync(response, cancellationToken);
    }
}


