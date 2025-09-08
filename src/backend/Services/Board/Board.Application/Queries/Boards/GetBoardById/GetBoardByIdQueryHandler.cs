using Board.Application.DTOs;
using Board.Application.Interfaces;
using MediatR;

namespace Board.Application.Queries.Boards.GetBoardById;

public class GetBoardByIdQueryHandler : IRequestHandler<GetBoardByIdQuery, BoardDto>
{
    private readonly IRepository<Domain.Entities.Board> _repository;

    public GetBoardByIdQueryHandler(IRepository<Domain.Entities.Board> repository)
    {
        _repository = repository;
    }

    public async Task<BoardDto> Handle(GetBoardByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetAsync(x => x.Id == request.Id, cancellationToken);
        if (entity == null)
        {
            return null;
        }

        return new BoardDto
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
    }
}
