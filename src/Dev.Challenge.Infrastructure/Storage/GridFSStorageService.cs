using Dev.Challenge.Application.Storage;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace Dev.Challenge.Infrastructure.Storage
{
    public class GridFSStorageService : IStorageService
    {
        private readonly IMongoDatabase _database;
        private readonly GridFSBucket _bucket;

        public GridFSStorageService(IMongoDatabase database)
        {
            _database = database;
            _bucket = new GridFSBucket(_database);
        }

        public async Task<string> UploadFileAsync(string filePath)
        {
            var fileName = Path.GetFileName(filePath);
            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                var fileId = await _bucket.UploadFromStreamAsync(fileName, fileStream);
                return fileId.ToString();
            }
        }

        public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
        {
            var fileId = await _bucket.UploadFromStreamAsync(fileName, fileStream);
            return fileId.ToString();
        }

        public async Task<Stream> DownloadFileAsync(string fileId)
        {
            var stream = new MemoryStream();
            await _bucket.DownloadToStreamAsync(new MongoDB.Bson.ObjectId(fileId), stream);
            stream.Position = 0;
            return stream;
        }
    }
}
