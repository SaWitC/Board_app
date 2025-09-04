using BoardAppApi.Data.Entities;
using BoardAppApi.Data.Repositories.Abstractions;
using BoardAppApi.Data.Repositories.Implemntations;
using MediatR;

namespace BoardAppApi.Features.BoardItems.DeleteBoardItems;

public class DeleteBoardItemHandler(IRepository<BoardItem> _rep) : IRequestHandler<DeleteBoardItemCommand, BoardItem>
{
    public async Task<BoardItem> Handle(DeleteBoardItemCommand request, CancellationToken cancellationToken)
    {
        var entity = await _rep.GetAsync(request.Id, cancellationToken);
        if (entity == null)
        {
            return null;
        }
        return await _rep.DeleteAsync(entity, cancellationToken);
    }
}


