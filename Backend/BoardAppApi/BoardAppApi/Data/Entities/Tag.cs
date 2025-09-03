using BoardAppApi.Data.Entities.Abstractions;

namespace BoardAppApi.Data.Entities
{
    public class Tag : IEntity
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public string Description { get; set; }
    } //TODO: Add index in title
}
