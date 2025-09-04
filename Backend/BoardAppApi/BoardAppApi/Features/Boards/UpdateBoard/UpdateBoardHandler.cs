using BoardAppApi.Data.Entities;
using BoardAppApi.Data.Repositories.Abstractions;
using MediatR;

namespace BoardAppApi.Features.Boards.UpdateBoard;

public class UpdateBoardHandler(IRepository<Board> _rep) : IRequestHandler<UpdateBoardCommand, Board>
{
    public async Task<Board> Handle(UpdateBoardCommand request, CancellationToken cancellationToken)
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


