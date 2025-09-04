using Board.Domain.Entities;
using Board.Application.Repositories;
using Board.Infrastructure.Data.Repositories.Abstractions;

namespace Board.Infrastructure.Data.Repositories.Implementations;

	public class BoardColumnRepository : BaseRepository<BoardColumn>, IBoardColumnRepository
	{
		public BoardColumnRepository(BoardDbContext context) : base(context)
		{
		}
	}
