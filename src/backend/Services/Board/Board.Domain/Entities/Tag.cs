using Board.Domain.Entities.Abstractions;

namespace Board.Domain.Entities;

public class Tag : IEntity
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public string Description { get; set; }
}
