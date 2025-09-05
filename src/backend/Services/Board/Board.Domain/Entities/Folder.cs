using Board.Domain.Entities.Abstractions;

namespace Board.Domain.Entities;

public class Folder : IEntity
{
    public Guid Id { get; set; }
    public BoardItem MasterItem { get; set; }
    public ICollection<BoardItem> SubItems { get; set; }
}
