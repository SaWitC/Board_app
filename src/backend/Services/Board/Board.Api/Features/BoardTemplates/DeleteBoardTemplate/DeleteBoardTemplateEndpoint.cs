using Board.Application.Abstractions.Repositories;
using Board.Domain.Entities;
using FastEndpoints;

namespace Board.Api.Features.BoardTemplates.DeleteBoardTemplate;

public class DeleteBoardTemplateEndpoint : EndpointWithoutRequest
{
    private readonly IBoardTemplateRepository _repository;

    public DeleteBoardTemplateEndpoint(IBoardTemplateRepository repository)
    {
        _repository = repository;
    }
    public override void Configure()
    {
        Delete("/api/boardtemplates/{id}");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        Guid id = Route<Guid>("id");
        BoardTemplate entity = await _repository.GetAsync(x => x.Id == id, ct, false);
        if (entity == null)
        {
            await Send.OkAsync(false, ct);
        }

        await _repository.DeleteAsync(entity, ct);
        await Send.OkAsync(true, ct);
    }
}
