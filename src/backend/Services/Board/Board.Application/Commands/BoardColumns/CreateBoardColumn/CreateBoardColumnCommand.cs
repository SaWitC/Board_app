using MediatR;
using Board.Application.DTOs;

namespace Board.Application.Commands.BoardColumns.CreateBoardColumn;

public class CreateBoardColumnCommand : IRequest<BoardColumnDto>
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public Guid BoardId { get; set; }
}
