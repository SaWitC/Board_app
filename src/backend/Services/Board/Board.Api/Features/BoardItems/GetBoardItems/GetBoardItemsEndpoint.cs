using Board.Application.Abstractions.Repositories;
using Board.Application.DTOs;
using Board.Domain.Entities;
using FastEndpoints;

namespace Board.Api.Features.BoardItems.GetBoardItems;

public class GetBoardItemsEndpoint : EndpointWithoutRequest
{
    private readonly IBoardItemRepository _repository;
    private readonly MapsterMapper.IMapper _mapper;

    public GetBoardItemsEndpoint(IBoardItemRepository repository, MapsterMapper.IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public override void Configure()
    {
        Get("/api/boarditems");
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        IList<BoardItem> entities = await _repository.GetAllAsync(cancellationToken, true);
        IList<BoardItemDto> items = _mapper.Map<IList<BoardItemDto>>(entities);
        await Send.OkAsync(items, cancellationToken);
    }
}

