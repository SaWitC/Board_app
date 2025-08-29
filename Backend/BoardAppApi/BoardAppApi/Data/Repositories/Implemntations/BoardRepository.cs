using BoardAppApi.Data.Entities;
using BoardAppApi.Data.Repositories.Abstractions;

namespace BoardAppApi.Data.Repositories.Implemntations
{
    public class BoardRepository : BaseRepository<Board>
    {
        public BoardRepository(BoardAppDbContext context) : base(context)
        {
        }
    }
}
