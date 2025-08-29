using BoardAppApi.Data.Entities;
using BoardAppApi.Data.Repositories.Abstractions;

namespace BoardAppApi.Data.Repositories.Implemntations
{
    public class BoardColumnRepository : BaseRepository<BoardColumn>
    {
        public BoardColumnRepository(BoardAppDbContext context) : base(context)
        {
        }
    }
}
