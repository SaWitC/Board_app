using Board.Domain.Entities.Abstractions;

namespace Board.Domain.Entities;

public class BoardColumn : IEntity
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public ICollection<BoardItem> Elements { get; set; }
    public Board Board { get; set; }
    public Guid BoardId { get; set; }
    public int Order { get; set; }
}
