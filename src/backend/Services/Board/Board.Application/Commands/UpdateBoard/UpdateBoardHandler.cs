using Board.Application.DTOs;
using Board.Application.Interfaces;
using MediatR;

namespace Board.Application.Commands.UpdateBoard;

public class UpdateBoardHandler : IRequestHandler<UpdateBoardCommand, BoardDto>
{
    private readonly IRepository<Domain.Entities.Board> _repository;

    public UpdateBoardHandler(IRepository<Domain.Entities.Board> repository)
    {
        _repository = repository;
    }

    public async Task<BoardDto> Handle(UpdateBoardCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetAsync(x => x.Id == request.Id, cancellationToken);
        if (entity == null)
        {
            return null;
        }

        entity.Title = request.Title;
        entity.Description = request.Description;
        entity.ModificationDate = DateTimeOffset.UtcNow;

        var updated = await _repository.UpdateAsync(entity, cancellationToken);
        return new BoardDto
        {
            Id = updated.Id,
            Title = updated.Title,
            Description = updated.Description,
            ModificationDate = updated.ModificationDate
        };
    }
}
