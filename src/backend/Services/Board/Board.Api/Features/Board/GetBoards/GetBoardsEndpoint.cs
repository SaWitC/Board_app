using Board.Application.Abstractions.Repositories;
using Board.Application.DTOs;
using FastEndpoints;

namespace Board.Api.Features.Board.GetBoards;

public class GetBoardsEndpoint : EndpointWithoutRequest
{
    public override void Configure()
    {
        Get("/api/boards");
    }

    private readonly IBoardRepository _repository;
    private readonly MapsterMapper.IMapper _mapper;

    public GetBoardsEndpoint(IBoardRepository repository, MapsterMapper.IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        IList<Domain.Entities.Board> entities = await _repository.GetAllAsync(cancellationToken, true, b => b.BoardColumns, b => b.BoardUsers);
        IList<BoardDto> boards = _mapper.Map<IList<BoardDto>>(entities);
        await Send.OkAsync(boards, cancellationToken);
    }
}

