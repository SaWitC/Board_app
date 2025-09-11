using Board.Application.DTOs;
using Board.Application.Interfaces;
using Board.Domain.Entities;
using FastEndpoints;

namespace Board.Api.Features.BoardTemplates.CreateBoardTemplate;

public class CreateBoardTemplateEndpoint : Endpoint<CreateBoardTemplateRequest>
{
    private readonly IRepository<BoardTemplate> _repository;

    public CreateBoardTemplateEndpoint(IRepository<BoardTemplate> repository)
    {
        _repository = repository;
    }
    public override void Configure()
    {
        Post("/api/boardtemplates");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateBoardTemplateRequest request, CancellationToken ct)
    {
        BoardTemplate entity = new BoardTemplate
        {
            Title = request.Title,
            Description = request.Description,
            Id = request.BoardId,
            IsActive = true
        };

        BoardTemplate created = await _repository.AddAsync(entity, ct);
        BoardTemplateDto response = new BoardTemplateDto
        {
            Title = created.Title,
            Description = created.Description,
            BoardId = created.Id
        };

        await Send.OkAsync(response, ct);
    }
}
