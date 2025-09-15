using Board.Application.Abstractions.Repositories;
using Board.Application.DTOs;
using Board.Domain.Entities;
using FastEndpoints;

namespace Board.Api.Features.BoardTemplates.GetBoardTemplateById;

public class GetBoardTemplateByIdEndpoint : EndpointWithoutRequest
{
    private readonly IBoardTemplateRepository _repository;

    public GetBoardTemplateByIdEndpoint(IBoardTemplateRepository repository)
    {
        _repository = repository;
    }
    public override void Configure()
    {
        Get("/api/boardtemplates/{id}");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        Guid id = Route<Guid>("id");
        BoardTemplate entity = await _repository.GetAsync(x => x.Id == id, ct);
        BoardTemplateDto response = entity == null
            ? null
            : new BoardTemplateDto
            {
                Title = entity.Title,
                Description = entity.Description,
                BoardId = entity.Id
            };

        await Send.OkAsync(response, ct);
    }
}
