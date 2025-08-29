using BoardAppApi.Data.Entities;
using BoardAppApi.Data.Repositories.Abstractions;

namespace BoardAppApi.Data.Repositories.Implemntations
{
    public class BoardItemRepository : BaseRepository<BoardItem>
    {
        public BoardItemRepository(BoardAppDbContext context) : base(context)
        {
        }
    }
}
