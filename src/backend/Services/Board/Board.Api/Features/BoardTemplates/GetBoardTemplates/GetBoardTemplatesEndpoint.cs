using Board.Application.Abstractions.Repositories;
using Board.Application.DTOs;
using FastEndpoints;

namespace Board.Api.Features.BoardTemplates.GetBoardTemplates;

public class GetBoardTemplatesEndpoint : EndpointWithoutRequest
{
    private readonly IBoardTemplateRepository _repository;

    public GetBoardTemplatesEndpoint(IBoardTemplateRepository repository)
    {
        _repository = repository;
    }
    public override void Configure()
    {
        Get("/api/boardtemplates");
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
