using Board.Application.Repositories;
using Board.Application.DTOs;
using MediatR;

namespace Board.Application.Commands.CreateBoard;

public class CreateBoardHandler : IRequestHandler<CreateBoardCommand, BoardDto>
{
    private readonly IBoardRepository _boardRepository;
    
    public CreateBoardHandler(IBoardRepository boardRepository)
    {
        _boardRepository = boardRepository;
    }
    
    public async Task<BoardDto> Handle(CreateBoardCommand request, CancellationToken cancellationToken)
    {
        var entity = new Domain.Entities.Board
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            BoardColumns = [],
            Admins = [],
            Owners = [],
            Users = [],
            ModificationDate = DateTimeOffset.UtcNow
        };

        var createdBoard = await _boardRepository.InsertAsync(entity, cancellationToken);
        
        return new BoardDto
        {
            Id = createdBoard.Id,
            Title = createdBoard.Title,
            Description = createdBoard.Description,
            ModificationDate = createdBoard.ModificationDate
        };
    }
}
