using Board.Application.DTOs;
using Board.Application.Interfaces;
using Board.Domain.Entities;
using FastEndpoints;

namespace Board.Api.Features.BoardTemplates.GetBoardTemplates;

public class GetBoardTemplatesEndpoint : EndpointWithoutRequest
{
    private readonly IRepository<BoardTemplate> _repository;

    public GetBoardTemplatesEndpoint(IRepository<BoardTemplate> repository)
    {
        _repository = repository;
    }
    public override void Configure()
    {
        Get("/api/boardtemplates");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        IList<BoardTemplateDto> templates = await _repository.GetAllAsync(null, t => new BoardTemplateDto
        {
            Title = t.Title,
            Description = t.Description,
            BoardId = t.Id
        }, ct);

        await Send.OkAsync(templates, ct);
    }
}
