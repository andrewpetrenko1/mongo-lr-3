using mongo_lr3.Models.Interfaces;
using MongoDB.Bson;

namespace mongo_lr3.Models
{
    public class Member : IEntity
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; } = default!;
        public int Age { get; set; }
        public string ImageId { get; set; } = default!;
        public bool HasImage() => !string.IsNullOrWhiteSpace(ImageId);
    }
}
