using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using HotelBookingSys.Application.Interfaces;

namespace HotelBookingSys.Infrastructure.Services;

public class AzureBlobStorageService : IImageStorageService
{
    private readonly BlobContainerClient _containerClient;

    public AzureBlobStorageService(string connectionString)
    {
        _containerClient = new BlobContainerClient(connectionString, "room-images");
    }

    /// <summary>
    /// Uploads an image to Azure Blob Storage and returns the blob URL.
    /// </summary>
    /// <param name="fileStream"></param>
    /// <param name="fileName"></param>
    /// <param name="contentType"></param>
    /// <returns></returns>
    public async Task<string> UploadAsync(Stream fileStream, string fileName, string contentType)
    {
        await _containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

        var blobName = Path.GetFileName(fileName);
        var blobClient = _containerClient.GetBlobClient(blobName);

        await blobClient.UploadAsync(fileStream, new BlobUploadOptions
        {
            HttpHeaders = new BlobHttpHeaders
            {
                ContentType = contentType
            }
        });
        return blobClient.Uri.ToString();
    }

    /// <summary>
    /// Deletes an image blob from Azure Blob Storage.
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public async Task DeleteAsync(string fileName)
    {
        var safeName = Path.GetFileName(fileName);
        var blobClient = _containerClient.GetBlobClient(safeName);
        await blobClient.DeleteIfExistsAsync();
    }
}
