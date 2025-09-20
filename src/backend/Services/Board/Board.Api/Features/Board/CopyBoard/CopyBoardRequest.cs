using Board.Application.DTOs;

namespace Board.Api.Features.Board.CopyBoard;

public sealed class CopyBoardRequest
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required Guid TemplateId { get; set; }
    public List<BoardUserDto> BoardUsers { get; set; } = [];
}
