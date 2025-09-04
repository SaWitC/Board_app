using Board.Domain.Entities;
using Board.Application.Repositories;
using Board.Infrastructure.Data.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Board.Infrastructure.Data.Repositories.Implementations;

	public class BoardItemRepository : BaseRepository<BoardItem>, IBoardItemRepository
	{
		public BoardItemRepository(BoardDbContext context) : base(context)
		{
		}

		public async Task<List<BoardItem>> GetAllAsync(CancellationToken cancellationToken)
		{
			return await context.Set<BoardItem>().ToListAsync(cancellationToken);
		}
	}
