using Board.Api.Configuration;
using Board.Application.Abstractions.Repositories;
using Board.Application.Abstractions.Services;
using Board.Application.DTOs;
using Board.Domain.Contracts.Enums;
using Board.Domain.Entities;
using Board.Domain.Security;
using FastEndpoints;
using IMapper = AutoMapper.IMapper;

namespace Board.Api.Features.Board.CopyBoard;

public sealed class CopyBoardEndpoint : Endpoint<CopyBoardRequest, CopyBoardResponse>
{
    private readonly IBoardRepository _repository;
    private readonly IMapper _mapper;
    private readonly ICurrentUserProvider _currentUserProvider;

    public CopyBoardEndpoint(IBoardRepository repository, IMapper mapper, ICurrentUserProvider currentUserProvider)
    {
        _repository = repository;
        _mapper = mapper;
        _currentUserProvider = currentUserProvider;
    }

    public override void Configure()
    {
        Post("/api/boards/copy");
        Policies(Auth.Policies.AuthenticatedUser);
    }
    
    private async Task<Domain.Entities.Board> GetTemplateBoardAsync(Guid templateId, CancellationToken cancellationToken)
    {
        var board = await _repository.GetAsync(
            b => b.Id == templateId, 
            cancellationToken, 
            false, 
            b => b.BoardColumns, 
            b => b.BoardColumns.Select(c => c.Elements), 
            b => b.BoardUsers);

        if (board == null)
        {
            throw new NotFoundException("Template board not found");
        }

        return board;
    }

    public override async Task<CopyBoardResponse> ExecuteAsync(CopyBoardRequest request, CancellationToken cancellationToken)
    {
        var templateBoard = await GetTemplateBoardAsync(request.TemplateId, cancellationToken);
        
        var newBoardId = Guid.NewGuid();
        
        var newBoard = new Domain.Entities.Board
        {
            Id = newBoardId,
            Title = request.Title,
            Description = request.Description,
            BoardUsers = request.BoardUsers.Select(userDto => new Domain.Entities.BoardUser
            {
                Email = userDto.Email,
                BoardId = newBoardId,
                Role = userDto.Role
            }).ToList(),
            BoardColumns = templateBoard.BoardColumns.Select(originalColumn => new Domain.Entities.BoardColumn
            {
                Id = Guid.NewGuid(),
                Title = originalColumn.Title,
                Description = originalColumn.Description,
                BoardId = newBoardId,
                Elements = originalColumn.Elements.Select(originalItem => new Domain.Entities.BoardItem
                {
                    Id = Guid.NewGuid(),
                    Title = originalItem.Title,
                    Description = originalItem.Description,
                    BoardColumnId = Guid.NewGuid(),
                    ModificationDate = DateTimeOffset.UtcNow,
                    Priority = originalItem.Priority,
                    AssigneeId = Guid.Empty,
                    DueDate = DateTime.MinValue,
                    CreatedTime = DateTime.UtcNow,
                    TaskType = originalItem.TaskType,
                    Tags = originalItem.Tags?.Select(tag => new Domain.Entities.Tag
                    {
                        Id = Guid.NewGuid(),
                        Title = tag.Title,
                        Description = tag.Description
                    }).ToList() ?? new List<Domain.Entities.Tag>(),
                    SubItems = new List<Domain.Entities.BoardItem>()
                }).ToList()
            }).ToList(),
            ModificationDate = DateTimeOffset.UtcNow
        };

        foreach (var column in newBoard.BoardColumns)
        {
            foreach (var item in column.Elements)
            {
                item.BoardColumnId = column.Id;
            }
        }

        var createdBoard = await _repository.AddAsync(newBoard, cancellationToken);
        var response = _mapper.Map<BoardDto>(createdBoard);

        return new CopyBoardResponse { Board = response };
    }
}
