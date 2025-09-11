using Board.Domain.Entities.Abstractions;

namespace Board.Domain.Entities;
public class BoardTemplate : IEntity
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public Guid BoardId { get; set; }
    public Board Board { get; set; }
    public bool IsActive { get; set; }
}
