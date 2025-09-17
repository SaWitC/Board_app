using Board.Api.Security;
using Board.Application.Abstractions.Repositories;
using Board.Application.Abstractions.Services;
using Board.Application.DTOs;
using Board.Domain.Entities;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using IMapper = AutoMapper.IMapper;

namespace Board.Api.Features.BoardItems.GetBoardItems;

[Authorize]
public class GetBoardItemsEndpoint : EndpointWithoutRequest
{
	private readonly IBoardItemRepository _repository;
	private readonly ICurrentUserProvider _currentUserProvider;
	private readonly IMapper _mapper;

	public GetBoardItemsEndpoint(IBoardItemRepository repository, ICurrentUserProvider currentUserProvider, IMapper mapper)
	{
		_repository = repository;
		_currentUserProvider = currentUserProvider;
		_mapper = mapper;
	}

	public override void Configure()
	{
		Get("/api/boarditems");
        Policies(Auth.Policies.AuthenticatedUser);
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
	{
		string email = _currentUserProvider.GetCurrentUserEmail();
		IList<BoardItem> entities = await _repository.GetAllAsync(
			predicate: i => i.BoardColumn.Board.BoardUsers.Any(u => u.Email == email),
			cancellationToken: cancellationToken,
			asNoTracking: true,
			i => i.BoardColumn,
			i => i.BoardColumn.Board,
			i => i.BoardColumn.Board.BoardUsers);
		IList<BoardItemDto> items = _mapper.Map<IList<BoardItemDto>>(entities);
		await Send.OkAsync(items, cancellationToken);
	}
}

