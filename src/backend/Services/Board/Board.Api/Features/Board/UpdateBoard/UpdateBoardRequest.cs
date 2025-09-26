using Board.Application.DTOs;

namespace Board.Api.Features.Board.UpdateBoard;

public class UpdateBoardRequest
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public List<BoardUserDto> BoardUsers { get; set; } = [];
    public List<BoardColumnDto> BoardColumns { get; set; } = [];
}
