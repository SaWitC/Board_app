using Board.Domain.Entities.Abstractions;

namespace Board.Domain.Entities;

public class BoardColumn : IEntity
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public ICollection<BoardItem> Elements { get; set; }
}
