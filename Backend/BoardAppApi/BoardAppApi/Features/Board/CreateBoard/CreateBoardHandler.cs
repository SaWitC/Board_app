using BoardAppApi.Data.Entities;
using BoardAppApi.Data.Repositories.Implemntations;
using MediatR;

namespace BoardAppApi.Features.Board.CreateBoard;

public class CreateBoardHandler(BoardRepository _rep) : IRequestHandler<CreateBoardCommand, Data.Entities.Board>
{
    public async Task<Data.Entities.Board> Handle(CreateBoardCommand request, CancellationToken cancellationToken)
    {
        var entity = new Data.Entities.Board
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


