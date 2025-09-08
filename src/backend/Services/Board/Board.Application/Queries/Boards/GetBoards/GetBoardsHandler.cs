using Board.Application.DTOs;
using Board.Application.Interfaces;
using MediatR;

namespace Board.Application.Queries.Boards.GetBoards;

public class GetBoardsHandler : IRequestHandler<GetBoardsQuery, List<BoardDto>>
{
    private readonly IRepository<Domain.Entities.Board> _repository;

    public GetBoardsHandler(IRepository<Domain.Entities.Board> repository)
    {
        _repository = repository;
    }

    public async Task<List<BoardDto>> Handle(GetBoardsQuery request, CancellationToken cancellationToken)
    {
        var boards = await _repository.GetAllAsync(null, b => new BoardDto
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

        return [.. boards];
    }
}
