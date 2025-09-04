using BoardAppApi.Data.Entities;
using MediatR;

namespace BoardAppApi.Features.Boards.GetBoards;

public class GetBoardsQuery : IRequest<List<Board>>
{
}


