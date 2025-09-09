using Board.Application.DTOs;
using Board.Application.Interfaces;
using Board.Domain.Entities;
using FastEndpoints;
using IMapper = AutoMapper.IMapper;

namespace Board.Api.Features.BoardItems.CreateBoardItem;

public class CreateBoardItemEndpoint : Endpoint<CreateBoardItemRequest>
{
    private readonly IRepository<BoardItem> _repository;
    private readonly IMapper _mapper;

    public CreateBoardItemEndpoint(IRepository<BoardItem> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public override void Configure()
    {
        Post("/api/boarditems");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateBoardItemRequest request, CancellationToken cancellationToken)
    {
        BoardItem entity = new BoardItem
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            BoardColumnId = request.BoardColumnId,
            Priority = request.Priority,
            AssigneeId = request.AssigneeId,
            DueDate = request.DueDate,
            CreatedTime = DateTime.UtcNow,
            ModificationDate = DateTimeOffset.UtcNow
        };

        BoardItem created = await _repository.AddAsync(entity, cancellationToken);
        var response = _mapper.Map<BoardItemDto>(created);

        await Send.OkAsync(response, cancellationToken);
    }
}


