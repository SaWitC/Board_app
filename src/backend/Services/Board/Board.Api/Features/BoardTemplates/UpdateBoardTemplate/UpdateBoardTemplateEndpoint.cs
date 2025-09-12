using Board.Application.DTOs;
using Board.Application.Interfaces;
using Board.Domain.Entities;
using FastEndpoints;

namespace Board.Api.Features.BoardTemplates.UpdateBoardTemplate;

public class UpdateBoardTemplateEndpoint : Endpoint<UpdateBoardTemplateRequest>
{
    private readonly IRepository<BoardTemplate> _repository;

    public UpdateBoardTemplateEndpoint(IRepository<BoardTemplate> repository)
    {
        _repository = repository;
    }
    public override void Configure()
    {
        Put("/api/boardtemplates/{id}");
    }

    public override async Task HandleAsync(UpdateBoardTemplateRequest request, CancellationToken ct)
    {
        Guid id = Route<Guid>("id");
        BoardTemplate entity = await _repository.GetAsync(x => x.Id == id, ct, false);
        if (entity == null)
        {
            await Send.OkAsync(null, ct);
        }

        entity.Title = request.Title;
        entity.Description = request.Description;
        entity.Id = request.BoardId;

        BoardTemplate updated = await _repository.UpdateAsync(entity, ct);
        BoardTemplateDto response = new BoardTemplateDto
        {
            Title = updated.Title,
            Description = updated.Description,
            BoardId = updated.Id
        };

        await Send.OkAsync(response, ct);
    }
}
