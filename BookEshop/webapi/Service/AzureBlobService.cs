using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using webapi.Models.UtilityModels;

namespace webapi.Service
{
    public class AzureBlobService : IAzureBlobService
    {
        private readonly string connectionString;
        private readonly BlobServiceClient blobServiceClient;
        private readonly BlobContainerClient bookPictureContainer ;
        public AzureBlobService(IConfiguration configuration) {
            this.connectionString = configuration.GetConnectionString("AzureBlobStorage");
            this.blobServiceClient = new BlobServiceClient(connectionString);
            this.bookPictureContainer = blobServiceClient.GetBlobContainerClient("books");
        }

        public async Task<string> UploadImageAsync(IFormFile picture)
        {
            if (picture == null || picture.Length == 0)
            {
                // Handle invalid or empty file
                return null;
            }

            string blobName = Guid.NewGuid().ToString() + Path.GetExtension(picture.FileName);
            var blobClient = bookPictureContainer.GetBlobClient(blobName);

            var options = new BlobUploadOptions
            {
                HttpHeaders = new BlobHttpHeaders { ContentType = picture.ContentType }
            };


            using (var stream = picture.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, new BlobUploadOptions
                {
                    HttpHeaders = new BlobHttpHeaders { ContentType = picture.ContentType }
                });
            }

            // Obtain the URL of the uploaded blob
            return blobClient.Uri.ToString();
        }

        public async Task<bool> RemoveImageAsync(string url)
        {
            if (url != "")
            {
                var blobUri = new Uri(url);
                var containerName = blobUri.Segments[1].Trim('/');
                var blobName = string.Join("", blobUri.Segments.Skip(2));

                var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
                var blobClient = containerClient.GetBlobClient(blobName);

                try
                {
                    var response = await blobClient.DeleteIfExistsAsync();

                    return response;
                } catch (Exception ex)
                {
                    throw new CustomException(StatusCodes.Status500InternalServerError, $"Nastala chyba pri mazani obrazku: {ex}");
                }

                return true;

                /*var blobClient = new BlobClient(new Uri(url));
                try
                {
                    var reasponse = await blobClient.
                    //var response = await blobClient.DeleteIfExistsAsync();
                    return response;

                }
                catch (Exception ex)
                {
                    throw new CustomException(StatusCodes.Status500InternalServerError, $"Nastala chyba pri mazani obrazku: {ex}");
                }*/


            } else
            {
                return false;
            }
        }
    }
}
