using Board.Application.Abstractions.Repositories;
using Board.Application.DTOs;
using Board.Domain.Contracts.Enums;
using Board.Domain.Contracts.Security;
using Board.Domain.Security;
using FastEndpoints;

namespace Board.Api.Features.BoardColumn.GetBoardColumns;

public class GetBoardColumnsEndpoint : EndpointWithoutRequest
{
    private readonly IBoardRepository _boardRepository;
    private readonly MapsterMapper.IMapper _mapper;

    public GetBoardColumnsEndpoint(IBoardRepository boardRepository, MapsterMapper.IMapper mapper)
    {
        _boardRepository = boardRepository;
        _mapper = mapper;
    }

    public override void Configure()
    {
        Get("/api/boards/{boardId}/columns");
        Policies(Auth.BuildPermissionPolicy(Permission.Read, Context.BoardColumn, "boardId"));
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
