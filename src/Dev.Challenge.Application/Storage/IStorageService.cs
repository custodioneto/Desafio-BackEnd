
namespace Dev.Challenge.Application.Storage
{
    public interface IStorageService
    {
        Task<string> UploadFileAsync(string filePath);
        Task<string> UploadFileAsync(Stream fileStream, string fileName);
        Task<Stream> DownloadFileAsync(string fileId);
    }

}
