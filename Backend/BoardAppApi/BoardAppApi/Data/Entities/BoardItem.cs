using BoardAppApi.Data.Entities.Abstractions;

namespace BoardAppApi.Data.Entities
{
    public class BoardItem : IEntity
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public ICollection<Tag> Tags { get; set; }
        public BoardColumn BoardColumn { get; set; }
        public Guid BoardColumnId { get; set; }
        public DateTimeOffset ModificationDate { get; set; }
    }
}
