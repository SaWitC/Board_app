using Board.Application.DTOs;
using Board.Application.Interfaces;
using Board.Domain.Entities;
using FastEndpoints;

namespace Board.Api.Features.BoardTemplates.GetBoardTemplateById;

public class GetBoardTemplateByIdEndpoint : EndpointWithoutRequest
{
    private readonly IRepository<BoardTemplate> _repository;

    public GetBoardTemplateByIdEndpoint(IRepository<BoardTemplate> repository)
    {
        _repository = repository;
    }
    public override void Configure()
    {
        Get("/api/boardtemplates/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        Guid id = Route<Guid>("id");
        BoardTemplate entity = await _repository.GetAsync(x => x.Id == id, ct);
        BoardTemplateDto response = entity == null
            ? null
            : new BoardTemplateDto
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
                BoardId = entity.BoardId
            };

        await Send.OkAsync(response, ct);
    }
}
