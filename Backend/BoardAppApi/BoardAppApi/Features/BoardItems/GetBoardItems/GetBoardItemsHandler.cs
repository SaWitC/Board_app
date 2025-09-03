using BoardAppApi.Data.Entities;
using BoardAppApi.Data.Repositories.Implemntations;
using MediatR;

namespace BoardAppApi.Features.BoardItems.GetBoardsItems;

public class GetBoardItemsHandler(BoardItemRepository _rep) : IRequestHandler<GetBoardItemsQuery, List<BoardItem>>
{
    public async Task<List<BoardItem>> Handle(GetBoardItemsQuery request, CancellationToken cancellationToken)
    {
        return await _rep.GetAllAsync(cancellationToken);
    }
}


