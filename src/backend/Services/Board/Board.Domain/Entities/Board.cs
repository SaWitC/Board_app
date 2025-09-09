using Board.Domain.Entities.Abstractions;

namespace Board.Domain.Entities;

public class Board : IEntity
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public ICollection<BoardUser> BoardUsers { get; set; } = [];
    public ICollection<BoardColumn> BoardColumns { get; set; } = [];
    public DateTimeOffset ModificationDate { get; set; }
}

