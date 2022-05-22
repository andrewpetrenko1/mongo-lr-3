using mongo_lr3.Models.Interfaces;

namespace mongo_lr3.Services.Interfaces
{
    public interface IFileService
    {
        Task<byte[]> GetImage(string id);
        Task<IEntity> StoreImage(IEntity entity, Stream imageStream, string imageName);
    }
}
