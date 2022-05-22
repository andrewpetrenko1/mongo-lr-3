using MongoDB.Bson;

namespace mongo_lr3.Models.Interfaces
{
    public interface IEntity
    {
        public ObjectId Id { get; set; }
        public string ImageId { get; set; }
        public bool HasImage();
    }
}
