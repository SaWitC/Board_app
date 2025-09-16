using Board.Application.Abstractions.Repositories;
using Board.Application.DTOs;
using FastEndpoints;
using IMapper = AutoMapper.IMapper;

namespace Board.Api.Features.BoardColumn.GetBoardColumns;

public class GetBoardColumnsEndpoint : EndpointWithoutRequest
{
    private readonly IBoardRepository _boardRepository;
    private readonly IMapper _mapper;

    public GetBoardColumnsEndpoint(IBoardRepository boardRepository, IMapper mapper)
    {
        _boardRepository = boardRepository;
        _mapper = mapper;
    }

    public override void Configure()
    {
        Get("/api/boards/{boardId}/columns");
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        Guid boardId = Route<Guid>("boardId");

        Domain.Entities.Board board = await _boardRepository.GetAsync(b => b.Id == boardId, cancellationToken, true, b => b.BoardColumns);
        if (board == null)
        {
            await Send.OkAsync(Array.Empty<BoardColumnDto>(), cancellationToken);
        }

        List<BoardColumnDto> response = _mapper.Map<List<BoardColumnDto>>(board.BoardColumns);

        await Send.OkAsync(response, cancellationToken);
    }
}
