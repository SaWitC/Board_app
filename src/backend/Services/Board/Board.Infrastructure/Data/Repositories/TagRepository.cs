using Board.Application.Abstractions.Repositories;
using Board.Application.DTOs.Tags;
using Board.Domain.Entities;
using FluentValidation;

namespace Board.Infrastructure.Data.Repositories;
public class TagRepository : Repository<Tag>, ITagRepository
{
    public TagRepository(BoardDbContext context) : base(context)
    {
    }

    public async Task<List<Tag>> GetOrCreateTagsAsync(IEnumerable<TagDto> tags, CancellationToken cancellationToken)
    {
        var newTagDtos = tags.Where(t => t.Id == Guid.Empty).ToList();
        var existingTagIds = tags.Where(t => t.Id != Guid.Empty).Select(t => t.Id).ToList();

        var existingTagsFromDb = existingTagIds.Count != 0
            ? await GetAllAsync(t => existingTagIds.Contains(t.Id), cancellationToken, false)
            : [];

        if (existingTagsFromDb.Count != existingTagIds.Count)
        {
            throw new ValidationException("One or more tags not found.");
        }

        var newTags = newTagDtos.Select(dto => new Tag
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            Description = dto.Description
        }).ToList();

        // Save new tags to the database before returning them
        if (newTags.Count > 0)
        {
            await _dbSet.AddRangeAsync(newTags, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        return [.. existingTagsFromDb, .. newTags];
    }
}

