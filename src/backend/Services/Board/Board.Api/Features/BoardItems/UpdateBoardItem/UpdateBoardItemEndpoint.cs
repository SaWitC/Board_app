using Board.Application.DTOs;
using Board.Application.Interfaces;
using Board.Domain.Entities;
using FastEndpoints;
using IMapper = AutoMapper.IMapper;

namespace Board.Api.Features.BoardItems.UpdateBoardItem;

public class UpdateBoardItemEndpoint : Endpoint<UpdateBoardItemRequest>
{
    private readonly IRepository<BoardItem> _repository;
    private readonly IMapper _mapper;

    public UpdateBoardItemEndpoint(IRepository<BoardItem> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public override void Configure()
    {
        Put("/api/boarditems/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateBoardItemRequest request, CancellationToken cancellationToken)
    {
        Guid id = Route<Guid>("id");

        BoardItem entity = await _repository.GetAsync(x => x.Id == id, cancellationToken);
        if (entity == null)
        {
            await Send.OkAsync(null, cancellationToken);
        }

        entity.Title = request.Title;
        entity.Description = request.Description;
        entity.BoardColumnId = request.BoardColumnId;
        entity.Priority = request.Priority;
        entity.AssigneeId = request.AssigneeId;
        entity.DueDate = request.DueDate;
        entity.ModificationDate = DateTimeOffset.UtcNow;

        BoardItem updated = await _repository.UpdateAsync(entity, cancellationToken);
        var response = _mapper.Map<BoardItemDto>(updated);

        await Send.OkAsync(response, cancellationToken);
    }
}
