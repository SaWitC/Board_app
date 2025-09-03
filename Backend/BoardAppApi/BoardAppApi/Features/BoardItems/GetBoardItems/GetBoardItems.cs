using BoardAppApi.Data.Entities;
using MediatR;

namespace BoardAppApi.Features.BoardItems.GetBoardsItems;

public class GetBoardItemsQuery : IRequest<List<BoardItem>>
{
}


