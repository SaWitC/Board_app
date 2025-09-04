using BoardAppApi.Data.Entities;
using BoardAppApi.Data.Repositories.Abstractions;
using MediatR;

namespace BoardAppApi.Features.Boards.GetBoardById;

public class GetBoardByIdQueryHandler(IRepository<Board> _rep) : IRequestHandler<GetBoardByIdQuery, Board>
{
    public async Task<Board> Handle(GetBoardByIdQuery request, CancellationToken cancellationToken)
    {
        return await _rep.GetAsync(request.Id, request.Ct);
    }
}


