using Board.Application.DTOs;
using Board.Application.Interfaces;
using FastEndpoints;

namespace Board.Api.Features.Board.CreateBoard;

public class CreateBoardEndpoint : Endpoint<CreateBoardRequest>
{
    private readonly IRepository<Domain.Entities.Board> _repository;

    public CreateBoardEndpoint(IRepository<Domain.Entities.Board> repository)
    {
        _repository = repository;
    }
    public override void Configure()
    {
        Post("/api/boards");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateBoardRequest request, CancellationToken cancellationToken)
    {
        // TODO: get guid from email
        List<Guid> userGuids = new List<Guid>();
        List<Guid> adminGuids = new List<Guid>();
        List<Guid> ownerGuids = new List<Guid>();

        List<Domain.Entities.BoardColumn> boardColumns = request.BoardColumns.Select(columnDto => new Domain.Entities.BoardColumn
        {
            Id = Guid.NewGuid(),
            Title = columnDto.Title,
            Description = columnDto.Description,
            Elements = []
        }).ToList();

        Domain.Entities.Board entity = new Domain.Entities.Board
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            BoardColumns = boardColumns,
            Users = userGuids,
            Admins = adminGuids,
            Owners = ownerGuids,
            ModificationDate = DateTimeOffset.UtcNow
        };

        Domain.Entities.Board createdBoard = await _repository.AddAsync(entity, cancellationToken);

        BoardDto response = new BoardDto
        {
            Id = createdBoard.Id,
            Title = createdBoard.Title,
            Description = createdBoard.Description,
            Users = request.Users,
            Admins = request.Admins,
            Owners = request.Owners,
            BoardColumns =
            [
                .. createdBoard.BoardColumns.Select(c => new BoardColumnDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    Description = c.Description
                })
            ],
            ModificationDate = createdBoard.ModificationDate
        };

        await Send.OkAsync(response, cancellationToken);
    }
}
