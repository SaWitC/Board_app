using Board.Application.Abstractions.Repositories;
using Board.Application.DTOs;
using FastEndpoints;
using IMapper = AutoMapper.IMapper;

namespace Board.Api.Features.Board.GetBoardById;

public class GetBoardByIdEndpoint : EndpointWithoutRequest
{
    public override void Configure()
    {
        Get("/api/boards/{id}");
    }

    private readonly IBoardRepository _repository;
    private readonly IMapper _mapper;

    public GetBoardByIdEndpoint(IBoardRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        Guid id = Route<Guid>("id");
        Domain.Entities.Board entity = await _repository.GetAsync(x => x.Id == id, cancellationToken, true, x => x.BoardColumns, x => x.BoardUsers, x => x.BoardTemplate);
        if (entity == null)
        {
            await Send.OkAsync(null, cancellationToken);
        }

        BoardDto response = _mapper.Map<BoardDto>(entity);
        response.IsActiveTemplate = entity?.BoardTemplate?.IsActive ?? false;
        response.IsTemplate = entity.BoardTemplate != null;

        await Send.OkAsync(response, cancellationToken);
    }
}

