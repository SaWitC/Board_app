using Board.Application.DTOs;

namespace Board.Api.Features.Board.CreateBoard;

public class CreateBoardRequest
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public List<string> Users { get; set; } = [];
    public List<string> Admins { get; set; } = [];
    public List<string> Owners { get; set; } = [];
    public List<BoardColumnDto> BoardColumns { get; set; } = [];
}
