using Board.Application.DTOs;
using Board.Application.Interfaces;
using Board.Domain.Entities;
using MediatR;

namespace Board.Application.Commands.CreateBoard;

public class CreateBoardHandler : IRequestHandler<CreateBoardCommand, BoardDto>
{
    private readonly IRepository<Domain.Entities.Board> _repository;

    public CreateBoardHandler(IRepository<Domain.Entities.Board> repository)
    {
        _repository = repository;
    }
    
    public async Task<BoardDto> Handle(CreateBoardCommand request, CancellationToken cancellationToken)
    {
        // TODO: get guid from email
        var userGuids = new List<Guid>();
        var adminGuids = new List<Guid>();
        var ownerGuids = new List<Guid>();

        var boardColumns = request.BoardColumns.Select(columnDto => new BoardColumn
        {
            Id = Guid.NewGuid(),
            Title = columnDto.Title,
            Description = columnDto.Description,
            Elements = []
        }).ToList();

        var entity = new Domain.Entities.Board
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            BoardColumns = boardColumns,
            Users = userGuids,
            Admins = adminGuids,
            Owners = ownerGuids,
            ModificationDate = DateTimeOffset.UtcNow
        };

        var createdBoard = await _repository.AddAsync(entity, cancellationToken);
        
        return new BoardDto
        {
            Id = createdBoard.Id,
            Title = createdBoard.Title,
            Description = createdBoard.Description,
            Users = request.Users,
            Admins = request.Admins,
            Owners = request.Owners,
            BoardColumns = [.. createdBoard.BoardColumns.Select(c => new BoardColumnDto
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description
            })],
            ModificationDate = createdBoard.ModificationDate
        };
    }
}
