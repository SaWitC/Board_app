using Board.Application.Interfaces;
using MediatR;

namespace Board.Application.Commands.DeleteBoard;

public class DeleteBoardHandler : IRequestHandler<DeleteBoardCommand, bool>
{
    private readonly IRepository<Domain.Entities.Board> _repository;

    public DeleteBoardHandler(IRepository<Domain.Entities.Board> repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteBoardCommand request, CancellationToken cancellationToken)
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
