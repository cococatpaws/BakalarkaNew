namespace webapi.Service
{
    public interface IAzureBlobService
    {
        Task<string> UploadImageAsync(IFormFile picture);
        Task<bool> RemoveImageAsync(string url);
    }
}
