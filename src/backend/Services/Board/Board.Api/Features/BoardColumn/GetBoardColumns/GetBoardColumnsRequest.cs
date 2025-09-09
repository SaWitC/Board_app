using Board.Application.DTOs;
using MediatR;

namespace Board.Api.Features.BoardColumn.GetBoardColumns;

public class GetBoardColumnsRequest : IRequest<List<BoardColumnDto>>
{
    public Guid BoardId { get; set; }
}
