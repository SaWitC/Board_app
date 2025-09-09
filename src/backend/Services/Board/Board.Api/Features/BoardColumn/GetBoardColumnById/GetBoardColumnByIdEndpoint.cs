using Board.Application.DTOs;
using Board.Application.Interfaces;
using FastEndpoints;
using IMapper = AutoMapper.IMapper;

namespace Board.Api.Features.BoardColumn.GetBoardColumnById;

public class GetBoardColumnByIdEndpoint : EndpointWithoutRequest
{

    private readonly IRepository<Domain.Entities.BoardColumn> _repository;
    private readonly IMapper _mapper;

    public GetBoardColumnByIdEndpoint(IRepository<Domain.Entities.BoardColumn> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public override void Configure()
    {
        Get("/api/boards/{boardId}/columns/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        Guid id = Route<Guid>("id");

        Domain.Entities.BoardColumn entity = await _repository.GetAsync(x => x.Id == id, cancellationToken);
        BoardColumnDto response = entity == null
            ? null
            : _mapper.Map<BoardColumnDto>(entity);

        await Send.OkAsync(response, cancellationToken);
    }
}
