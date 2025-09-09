using Board.Application.DTOs;
using Board.Application.Interfaces;
using FastEndpoints;

namespace Board.Api.Features.Board.GetBoardById;

public class GetBoardByIdEndpoint : EndpointWithoutRequest
{
    public override void Configure()
    {
        Get("/api/boards/{id}");
        AllowAnonymous();
    }

    private readonly IRepository<Domain.Entities.Board> _repository;

    public GetBoardByIdEndpoint(IRepository<Domain.Entities.Board> repository)
    {
        _repository = repository;
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        Guid id = Route<Guid>("id");
        Domain.Entities.Board entity = await _repository.GetAsync(x => x.Id == id, cancellationToken);
        if (entity == null)
        {
            await Send.OkAsync(null, cancellationToken);
        }

        BoardDto response = new BoardDto
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description,
            //TODO: add guid -> email mapping
            Users = [.. entity.Users.Select(u => u.ToString())],
            Admins = [.. entity.Admins.Select(a => a.ToString())],
            Owners = [.. entity.Owners.Select(o => o.ToString())],
            ModificationDate = entity.ModificationDate
        };

        await Send.OkAsync(response, cancellationToken);
    }
}


