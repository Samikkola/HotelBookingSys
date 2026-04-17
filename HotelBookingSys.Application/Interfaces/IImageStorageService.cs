namespace HotelBookingSys.Application.Interfaces;

/// <summary>
/// Defines file storage operations for room images.
/// </summary>
public interface IImageStorageService
{
    /// <summary>
    /// Uploads an image file to the underlying storage provider and returns its public URL.
    /// </summary>
    /// <param name="fileStream"></param>
    /// <param name="fileName"></param>
    /// <param name="contentType"></param>
    /// <returns></returns>
    Task<string> UploadAsync(Stream fileStream, string fileName, string contentType);

    /// <summary>
    /// Deletes an image file from the underlying storage provider.
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    Task DeleteAsync(string fileName);
}
