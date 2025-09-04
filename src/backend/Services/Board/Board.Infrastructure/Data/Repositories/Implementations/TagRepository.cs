using Board.Domain.Entities;
using Board.Application.Repositories;
using Board.Infrastructure.Data.Repositories.Abstractions;

namespace Board.Infrastructure.Data.Repositories.Implementations;

	public class TagRepository : BaseRepository<Tag>, ITagRepository
	{
		public TagRepository(BoardDbContext context) : base(context)
		{
		}
	}
