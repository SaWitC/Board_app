using Board.Application.Abstractions.Repositories;
using Board.Application.Abstractions.Services;
using Board.Application.DTOs;
using Board.Domain.Contracts.Enums;
using Board.Domain.Contracts.Pagination;
using Board.Domain.Security;
using FastEndpoints;
using LinqKit;
using Microsoft.AspNetCore.Authorization;
using IMapper = AutoMapper.IMapper;

namespace Board.Api.Features.Board.GetBoards;

[Authorize]
public class GetBoardsEndpoint : Endpoint<GetBoardsRequest>
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
    public override async Task HandleAsync(GetBoardsRequest request, CancellationToken cancellationToken)
    {
        string email = _currentUserProvider.GetUserEmail();
        bool isGlobalAdmin = _currentUserProvider.IsGlobalAdmin();
        var predicate = PredicateBuilder.New<Domain.Entities.Board>(true);

        // Apply user access filter
        if (!isGlobalAdmin)
        {
            predicate = predicate.And(b => b.BoardUsers.Any(u => u.Email == email));
        }

        // Apply search filter
        if (!string.IsNullOrWhiteSpace(request.TitleSearchTerm))
        {
            predicate = predicate.And(b => b.Title.Contains(request.TitleSearchTerm));
        }
        if (!string.IsNullOrWhiteSpace(request.OwnerSearchTerm))
        {
            predicate = predicate.And(b => b.BoardUsers.Any(u => u.Role == UserAccessEnum.BoardOwner && u.Email.Contains(request.OwnerSearchTerm)));
        }

        PagedResult<Domain.Entities.Board> entities = await _repository.GetPagedAsync(
            request.Page,
            request.PageSize,
            predicate,
            q => q.OrderBy(u => u.Title),
            cancellationToken, 
            true, 
            b => b.BoardColumns, 
            b => b.BoardUsers
        );

        var result = _mapper.Map<PagedResult<BoardDto>>(entities);

        await Send.OkAsync(result, cancellationToken);
    }
}

