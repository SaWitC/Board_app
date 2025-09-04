using BoardAppApi.Data.Entities;
using BoardAppApi.Data.Repositories.Abstractions;
using MediatR;

namespace BoardAppApi.Features.Boards.DeleteBoard;

public class DeleteBoardHandler(IRepository<Board> _rep) : IRequestHandler<DeleteBoardCommand, Board>
{
    public async Task<Board> Handle(DeleteBoardCommand request, CancellationToken cancellationToken)
    {
        var entity = await _rep.GetAsync(request.Id, cancellationToken);
        if (entity == null)
        {
            return null;
        }
        return await _rep.DeleteAsync(entity, cancellationToken);
    }
}


