using Board.Api.Configuration;
using Board.Application.Abstractions.Repositories;
using Board.Application.Abstractions.Services;
using Board.Application.DTOs;
using Board.Domain.Contracts.Enums;
using Board.Domain.Security;
using FastEndpoints;
using IMapper = AutoMapper.IMapper;

namespace Board.Api.Features.Board.CreateBoard;

public class CreateBoardEndpoint : Endpoint<CreateBoardRequest>
{
    private readonly IBoardRepository _repository;
    private readonly IMapper _mapper;
    private readonly ICurrentUserProvider _currentUserProvider;

    public CreateBoardEndpoint(IBoardRepository repository, IMapper mapper, ICurrentUserProvider currentUserProvider)
    {
        _repository = repository;
        _mapper = mapper;
        _currentUserProvider = currentUserProvider;
    }
    public override void Configure()
    {
        Post("/api/boards");
        Policies(Auth.Policies.AuthenticatedUser);
    }

    public override async Task HandleAsync(CreateBoardRequest request, CancellationToken cancellationToken)
    {
        string currentEmail = _currentUserProvider.GetUserEmail();

        var boardOwner = request.BoardUsers.FirstOrDefault(u => u.Role == UserAccessEnum.BoardOwner);
        if (boardOwner==null)
        {
            request.BoardUsers ??= [];
            request.BoardUsers.Add(new BoardUserDto { Email = currentEmail, Role = UserAccessEnum.BoardOwner });
        }
        else
        {
            if (boardOwner.Email != currentEmail&&!_currentUserProvider.IsGlobalAdmin())
            {
                throw new ForbiddenAccessException("You can not create board for other users");
            }
        }

        Guid boardId = Guid.NewGuid();
        Domain.Entities.Board entity = new()
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

        Domain.Entities.Board createdBoard = await _repository.AddAsync(entity, cancellationToken);

        BoardDto response = _mapper.Map<BoardDto>(createdBoard);

        await Send.OkAsync(response, cancellationToken);
    }
}
