using Board.Application.DTOs;
using Board.Application.Interfaces;
using FastEndpoints;
using IMapper = AutoMapper.IMapper;

namespace Board.Api.Features.Board.CreateBoard;

public class CreateBoardEndpoint : Endpoint<CreateBoardRequest>
{
    private readonly IRepository<Domain.Entities.Board> _repository;
    private readonly IMapper _mapper;

    public CreateBoardEndpoint(IRepository<Domain.Entities.Board> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public override void Configure()
    {
        Post("/api/boards");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateBoardRequest request, CancellationToken cancellationToken)
    {
        var boardId = Guid.NewGuid();
        var entity = new Domain.Entities.Board
        {
            Id = boardId,
            Title = request.Title,
            Description = request.Description,
            BoardColumns = [.. request.BoardColumns.Select(columnDto => new Domain.Entities.BoardColumn
            {
                Id = Guid.NewGuid(),
                Title = columnDto.Title,
                Description = columnDto.Description,
                Elements = []
            })],
            BoardUsers = [.. request.BoardUsers.Select(userDto => new Domain.Entities.BoardUser
            {
                Email = userDto.Email,
                BoardId = boardId,
                Role = userDto.Role
            })],
            ModificationDate = DateTimeOffset.UtcNow
        };

        var createdBoard = await _repository.AddAsync(entity, cancellationToken);

        var response = _mapper.Map<BoardDto>(createdBoard);

        await Send.OkAsync(response, cancellationToken);
    }
}
