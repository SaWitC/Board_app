using BoardAppApi.Data.Entities;
using BoardAppApi.Data.Repositories.Implemntations;
using MediatR;

namespace BoardAppApi.Features.Board.DeleteBoard;

public class DeleteBoardHandler(BoardRepository _rep) : IRequestHandler<DeleteBoardCommand, Data.Entities.Board>
{
    public async Task<Data.Entities.Board> Handle(DeleteBoardCommand request, CancellationToken cancellationToken)
    {
        var entity = await _rep.GetAsync(request.Id, cancellationToken);
        if (entity == null)
        {
            return null;
        }
        return await _rep.DeleteAsync(entity, cancellationToken);
    }
}


