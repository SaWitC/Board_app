using Board.Application.DTOs.Tags;
using Board.Domain.Entities;

namespace Board.Application.Abstractions.Repositories;

public interface ITagRepository : IRepository<Tag>
{
    Task<List<Tag>> GetOrCreateTagsAsync(IEnumerable<TagDto> tags, CancellationToken cancellationToken);
}
