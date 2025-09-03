using BoardAppApi.Data.Entities;
using BoardAppApi.Data.Repositories.Implemntations;
using MediatR;

namespace BoardAppApi.Features.BoardItems.GetBoardById;

public class GetBoardItemByIdQueryHandler(BoardItemRepository _rep) : IRequestHandler<GetBoardItemByIdQuery, BoardItem>
{
    public async Task<BoardItem> Handle(GetBoardItemByIdQuery request, CancellationToken cancellationToken)
    {
        return await _rep.GetAsync(request.Id, request.Ct);
    }
}
