using Board.Application.DTOs;

namespace Board.Api.Features.Board.UpdateBoardUsers;

public class UpdateBoardUsersRequest
{
    public required List<BoardUserDto> BoardUsers { get; set; }
}
