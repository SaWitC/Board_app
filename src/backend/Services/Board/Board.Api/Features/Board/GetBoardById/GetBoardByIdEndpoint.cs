using Board.Application.DTOs;
using Board.Application.Interfaces;
using FastEndpoints;
using IMapper = AutoMapper.IMapper;

namespace Board.Api.Features.Board.GetBoardById;

public class GetBoardByIdEndpoint : EndpointWithoutRequest
{
    public override void Configure()
    {
        Get("/api/boards/{id}");
    }

    private readonly IRepository<Domain.Entities.Board> _repository;
    private readonly IMapper _mapper;

    public GetBoardByIdEndpoint(IRepository<Domain.Entities.Board> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        Guid id = Route<Guid>("id");
        Domain.Entities.Board entity = await _repository.GetAsync(x => x.Id == id, cancellationToken, true, x => x.BoardColumns, x => x.BoardUsers);
        if (entity == null)
        {
            await Send.OkAsync(null, cancellationToken);
        }

        var response = _mapper.Map<BoardDto>(entity);
        await Send.OkAsync(response, cancellationToken);
    }
}

