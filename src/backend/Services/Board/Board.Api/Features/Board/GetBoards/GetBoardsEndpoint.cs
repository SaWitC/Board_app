using Board.Application.DTOs;
using Board.Application.Interfaces;
using FastEndpoints;

namespace Board.Api.Features.Board.GetBoards;

public class GetBoardsEndpoint : EndpointWithoutRequest
{
    public override void Configure()
    {
        Get("/api/boards");
        AllowAnonymous();
    }

    private readonly IRepository<Domain.Entities.Board> _repository;

    public GetBoardsEndpoint(IRepository<Domain.Entities.Board> repository)
    {
        _repository = repository;
    }
    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        IList<BoardDto> boards = await _repository.GetAllAsync(null, b => new BoardDto
        {
            Id = b.Id,
            Title = b.Title,
            Description = b.Description,
            //TODO: add guid -> email mapping
            Users = b.Users.Select(u => u.ToString()).ToList(),
            Admins = b.Admins.Select(a => a.ToString()).ToList(),
            Owners = b.Owners.Select(o => o.ToString()).ToList(),
            ModificationDate = b.ModificationDate
        }, cancellationToken);

        await Send.OkAsync(boards, cancellationToken);
    }
}


