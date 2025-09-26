using Board.Application.Abstractions.Repositories;
using Board.Application.DTOs;
using Board.Domain.Entities;
using FastEndpoints;

namespace Board.Api.Features.BoardTemplates.CreateBoardTemplate;

public class CreateBoardTemplateEndpoint : Endpoint<CreateBoardTemplateRequest>
{
    private readonly IBoardTemplateRepository _repository;

    public CreateBoardTemplateEndpoint(IBoardTemplateRepository repository)
    {
        _repository = repository;
    }
    public override void Configure()
    {
        Post("/api/boardtemplates");
    }

    public override async Task HandleAsync(CreateBoardTemplateRequest request, CancellationToken ct)
    {
        BoardTemplate entity = new()
        {
            Title = request.Title,
            Description = request.Description,
            Id = request.BoardId,
            IsActive = true
        };

        BoardTemplate created = await _repository.AddAsync(entity, ct);
        BoardTemplateDto response = new()
        {
            Title = created.Title,
            Description = created.Description,
            BoardId = created.Id
        };

        await Send.OkAsync(response, ct);
    }
}
