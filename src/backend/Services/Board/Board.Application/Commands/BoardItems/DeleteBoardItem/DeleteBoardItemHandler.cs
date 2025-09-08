using Board.Application.Interfaces;
using Board.Domain.Entities;
using MediatR;

namespace Board.Application.Commands.BoardItems.DeleteBoardItem;

public class DeleteBoardItemHandler : IRequestHandler<DeleteBoardItemCommand, bool>
{
    private readonly IRepository<BoardItem> _repository;

    public DeleteBoardItemHandler(IRepository<BoardItem> repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteBoardItemCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetAsync(x => x.Id == request.Id, cancellationToken);
        if (entity == null)
        {
            return false;
        }

        await _repository.DeleteAsync(entity, cancellationToken);
        return true;
    }
}
