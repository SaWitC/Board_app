using Board.Api.Security;
using Board.Application.Abstractions.Repositories;
using Board.Application.Abstractions.Services;
using Board.Application.DTOs;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using IMapper = AutoMapper.IMapper;

namespace Board.Api.Features.Board.GetBoards;

[Authorize]
public class GetBoardsEndpoint : EndpointWithoutRequest
{
    private readonly IBoardRepository _repository;
    private readonly IMapper _mapper;
    private readonly ICurrentUserProvider _currentUserProvider;

    public override void Configure()
    {
        Get("/api/boards");
        Policies(Auth.Policies.AuthenticatedUser);
    }

    public GetBoardsEndpoint(IBoardRepository repository, IMapper mapper, ICurrentUserProvider currentUserProvider)
    {
        _repository = repository;
        _mapper = mapper;
        _currentUserProvider = currentUserProvider;
    }
    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        string email = _currentUserProvider.GetCurrentUserEmail();
        IList<Domain.Entities.Board> entities = await _repository.GetAllAsync(b => b.BoardUsers.Any(u => u.Email == email), cancellationToken, true, b => b.BoardColumns, b => b.BoardUsers);
        IList<BoardDto> boards = _mapper.Map<IList<BoardDto>>(entities);
        await Send.OkAsync(boards, cancellationToken);
    }
}

