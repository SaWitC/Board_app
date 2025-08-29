using BoardAppApi.Data.Entities;
using BoardAppApi.Data.Repositories.Abstractions;

namespace BoardAppApi.Data.Repositories.Implemntations
{
    public class TagRepository : BaseRepository<Tag>
    {
        public TagRepository(BoardAppDbContext context) : base(context)
        {
        }
    }
}
