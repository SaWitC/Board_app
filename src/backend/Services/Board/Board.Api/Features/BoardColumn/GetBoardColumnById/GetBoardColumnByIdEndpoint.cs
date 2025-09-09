using Board.Application.DTOs;
using Board.Application.Interfaces;
using FastEndpoints;

namespace Board.Api.Features.BoardColumn.GetBoardColumnById;

public class GetBoardColumnByIdEndpoint : EndpointWithoutRequest
{

    private readonly IRepository<Domain.Entities.BoardColumn> _repository;

    public GetBoardColumnByIdEndpoint(IRepository<Domain.Entities.BoardColumn> repository)
    {
        _repository = repository;
    }
    public override void Configure()
    {
        Get("/api/boards/{boardId}/columns/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        Guid id = Route<Guid>("id");

        Domain.Entities.BoardColumn entity = await _repository.GetAsync(x => x.Id == id, cancellationToken);
        BoardColumnDto response = entity == null
            ? null
            : new BoardColumnDto
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description
            };

        await Send.OkAsync(response, cancellationToken);
    }
}
