using BoardAppApi.Data.Entities;
using MediatR;

namespace BoardAppApi.Features.Board.GetBoards;

public class GetBoardsQuery : IRequest<List<Data.Entities.Board>>
{
}


