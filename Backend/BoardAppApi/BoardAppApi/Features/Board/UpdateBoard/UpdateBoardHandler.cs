using BoardAppApi.Data.Entities;
using BoardAppApi.Data.Repositories.Implemntations;
using MediatR;

namespace BoardAppApi.Features.Board.UpdateBoard;

public class UpdateBoardHandler(BoardRepository _rep) : IRequestHandler<UpdateBoardCommand, Data.Entities.Board>
{
    public async Task<Data.Entities.Board> Handle(UpdateBoardCommand request, CancellationToken cancellationToken)
    {
        var entity = await _rep.GetAsync(request.Id, cancellationToken);
        if (entity == null)
        {
            return null;
        }

        entity.Title = request.Title;
        entity.Description = request.Description;
        entity.ModificationDate = DateTimeOffset.UtcNow;

        return await _rep.UpdateAsync(entity, cancellationToken);
    }
}


