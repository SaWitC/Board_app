using BoardAppApi.Data.Entities;
using BoardAppApi.Data.Repositories.Implemntations;
using MediatR;

namespace BoardAppApi.Features.Board.GetBoards;

public class GetBoardsHandler(BoardRepository _rep) : IRequestHandler<GetBoardsQuery, List<Data.Entities.Board>>
{
    public async Task<List<Data.Entities.Board>> Handle(GetBoardsQuery request, CancellationToken cancellationToken)
    {
        return await _rep.GetAllAsync(cancellationToken);
    }
}


