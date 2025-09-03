using BoardAppApi.Data.Entities;
using BoardAppApi.Data.Repositories.Implemntations;
using MediatR;

namespace BoardAppApi.Features.Board.GetBoardById;

public class GetBoardByIdQueryHandler(BoardRepository _rep) : IRequestHandler<GetBoardByIdQuery, Data.Entities.Board>
{
    public async Task<Data.Entities.Board> Handle(GetBoardByIdQuery request, CancellationToken cancellationToken)
    {
        return await _rep.GetAsync(request.Id, request.Ct);
    }
}


