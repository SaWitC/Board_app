using Board.Application.DTOs;
using Board.Application.Interfaces;
using FastEndpoints;
using IMapper = AutoMapper.IMapper;

namespace Board.Api.Features.Board.GetBoards;

public class GetBoardsEndpoint : EndpointWithoutRequest
{
    public override void Configure()
    {
        Get("/api/boards");
    }

    private readonly IRepository<Domain.Entities.Board> _repository;
    private readonly IMapper _mapper;

    public GetBoardsEndpoint(IRepository<Domain.Entities.Board> repository, IMapper mapper)
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

