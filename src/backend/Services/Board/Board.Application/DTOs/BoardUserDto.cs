using Board.Domain.Contracts.Enums;

namespace Board.Application.DTOs;

public class BoardUserDto
{
    public required string Email { get; set; }

    public required UserAccessEnum Role { get; set; }
} 
