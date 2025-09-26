using Board.Application.DTOs;

namespace Board.Api.Features.BoardColumn.UpdateBoardColumnOrder;

public class UpdateBoardColumnsOrderRequest
{
    public OrderedColumnDto[] Columns { get; set; }
}
