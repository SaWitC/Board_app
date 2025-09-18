using Board.Application.Abstractions.Repositories;
using Board.Application.Abstractions.Services;
using Board.Application.DTOs;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;

namespace Board.Api.Features.Board.GetBoards;

[Authorize]
public class GetBoardsEndpoint : EndpointWithoutRequest
{
    private readonly IBoardRepository _repository;
    private readonly MapsterMapper.IMapper _mapper;
    private readonly ICurrentUserProvider _currentUserProvider;

    public override void Configure()
    {
        Get("/api/boards");
    }

    public GetBoardsEndpoint(IBoardRepository repository, MapsterMapper.IMapper mapper, ICurrentUserProvider currentUserProvider)
    {
        _repository = repository;
        _mapper = mapper;
        _currentUserProvider = currentUserProvider;
    }
    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        string email = _currentUserProvider.GetUserEmail();
        IList<Domain.Entities.Board> entities = await _repository.GetAllAsync(b => b.BoardUsers.Any(u => u.Email == email), cancellationToken, true, b => b.BoardColumns, b => b.BoardUsers);
        IList<BoardDto> boards = _mapper.Map<IList<BoardDto>>(entities);
        await Send.OkAsync(boards, cancellationToken);
    }
}

