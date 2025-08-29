using BoardAppApi.Data.Entities.Abstractions;

namespace BoardAppApi.Data.Entities
{
    public class Board: IEntity
    {
        public Guid Id { get; set; }
        public required string Title {  get; set; }
        public required string Description {  get; set; }
        public ICollection<Guid> Users {  get; set; }
        public ICollection<Guid> Admins {  get; set; }
        public ICollection<Guid> Owners {  get; set; }
        public ICollection<BoardColumn> BoardColumns { get; set; }
        public DateTimeOffset ModificationDate { get; set; }
    }
}
