using BoardAppApi.Data.Entities;
using BoardAppApi.Data.Repositories.Abstractions;
using MediatR;

namespace BoardAppApi.Features.Boards.CreateBoard;

public class CreateBoardHandler(IRepository<Board> _rep) : IRequestHandler<CreateBoardCommand, Board>
{
    public async Task<Board> Handle(CreateBoardCommand request, CancellationToken cancellationToken)
    {
        var entity = new Board
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            BoardColumns = new List<BoardColumn>(),
            Admins = new List<Guid>(),
            Owners = new List<Guid>(),
            Users = new List<Guid>(),
            ModificationDate = DateTimeOffset.UtcNow
        };

        return await _rep.InsertAsync(entity, cancellationToken);
    }
}


