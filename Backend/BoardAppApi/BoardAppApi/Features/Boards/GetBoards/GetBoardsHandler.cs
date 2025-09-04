using BoardAppApi.Data.Entities;
using BoardAppApi.Data.Repositories.Abstractions;
using BoardAppApi.Data.Repositories.Implemntations;
using MediatR;

namespace BoardAppApi.Features.Boards.GetBoards;

public class GetBoardsHandler(BoardRepository _rep) : IRequestHandler<GetBoardsQuery, List<Board>>
{
    public async Task<List<Board>> Handle(GetBoardsQuery request, CancellationToken cancellationToken)
    {
        return await _rep.GetAllAsync(cancellationToken);
    }
}


