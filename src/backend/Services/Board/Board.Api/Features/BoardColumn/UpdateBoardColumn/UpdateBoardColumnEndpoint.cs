using Board.Application.DTOs;
using Board.Application.Interfaces;
using FastEndpoints;
using IMapper = AutoMapper.IMapper;

namespace Board.Api.Features.BoardColumn.UpdateBoardColumn;

public class UpdateBoardColumnEndpoint : Endpoint<UpdateBoardColumnRequest>
{
    private readonly IRepository<Domain.Entities.BoardColumn> _repository;
    private readonly IMapper _mapper;

    public UpdateBoardColumnEndpoint(IRepository<Domain.Entities.BoardColumn> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public override void Configure()
    {
        Put("/api/boards/{boardId}/columns");
    }

    public override async Task HandleAsync(UpdateBoardColumnRequest request, CancellationToken cancellationToken)
    {
        Domain.Entities.BoardColumn entity = await _repository.GetAsync(x => x.Id == request.Id, cancellationToken, false);
        if (entity == null)
        {
            throw new InvalidOperationException("Column not found");
        }

        entity.Title = request.Title;
        entity.Description = request.Description;

        Domain.Entities.BoardColumn updated = await _repository.UpdateAsync(entity, cancellationToken);

        var response = _mapper.Map<BoardColumnDto>(updated);
        await Send.OkAsync(response, cancellationToken);
    }
}
