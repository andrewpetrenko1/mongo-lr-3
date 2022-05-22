using mongo_lr3.Models.Interfaces;
using mongo_lr3.Services.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver.GridFS;

namespace mongo_lr3.Services
{
    public class FileService : IFileService
    {
        private readonly IGridFSBucket _gridFS;
        public FileService(IGridFSBucket gridFSBucket)
        {
            _gridFS = gridFSBucket ?? throw new ArgumentNullException(nameof(gridFSBucket));
        }

        public async Task<byte[]> GetImage(string id) => await _gridFS.DownloadAsBytesAsync(new ObjectId(id));

        public async Task<IEntity> StoreImage(IEntity entity, Stream imageStream, string imageName)
        {
            if(entity.HasImage())
                await _gridFS.DeleteAsync(new ObjectId(entity.ImageId));

            var imageId = await _gridFS.UploadFromStreamAsync(imageName, imageStream);
            entity.ImageId = imageId.ToString();
            return entity;
        }
    }
}
