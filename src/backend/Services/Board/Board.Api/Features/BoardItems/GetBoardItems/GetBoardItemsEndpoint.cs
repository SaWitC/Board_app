using Board.Application.DTOs;
using Board.Application.Interfaces;
using Board.Domain.Entities;
using FastEndpoints;
using IMapper = AutoMapper.IMapper;

namespace Board.Api.Features.BoardItems.GetBoardItems;

public class GetBoardItemsEndpoint : EndpointWithoutRequest
{
    private readonly IRepository<BoardItem> _repository;
    private readonly IMapper _mapper;

    public GetBoardItemsEndpoint(IRepository<BoardItem> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public override void Configure()
    {
        Get("/api/boarditems");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        var entities = await _repository.GetAllAsync(cancellationToken, true);
        var items = _mapper.Map<IList<BoardItemDto>>(entities);
        await Send.OkAsync(items, cancellationToken);
    }
}

