using BoardAppApi.Data.Entities.Abstractions;

namespace BoardAppApi.Data.Entities;

public class Folder : IEntity
{
    public Guid Id { get; set; }
    public BoardItem MasterItem { get; set; }
    public ICollection<BoardItem> SubItems { get; set; }
}
