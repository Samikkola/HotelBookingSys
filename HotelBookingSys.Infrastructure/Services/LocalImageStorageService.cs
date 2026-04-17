using HotelBookingSys.Application.Interfaces;
using Microsoft.AspNetCore.Hosting;

namespace HotelBookingSys.Infrastructure.Services;

public class LocalImageStorageService : IImageStorageService
{
    private const string RelativeFolder = "images/rooms";
    private readonly string _absoluteFolderPath;

    public LocalImageStorageService(IWebHostEnvironment environment)
    {
        var webRoot = environment.WebRootPath;
        if (string.IsNullOrWhiteSpace(webRoot))
        {
            webRoot = Path.Combine(environment.ContentRootPath, "wwwroot");
        }

        _absoluteFolderPath = Path.Combine(webRoot, "images", "rooms");
        Directory.CreateDirectory(_absoluteFolderPath);
    }

    /// <summary>
    /// Uploads an image to local development storage.
    /// </summary>
    /// <param name="fileStream"></param>
    /// <param name="fileName"></param>
    /// <param name="contentType"></param>
    /// <returns></returns>
    public async Task<string> UploadAsync(Stream fileStream, string fileName, string contentType)
    {
        var safeFileName = Path.GetFileName(fileName);
        var path = Path.Combine(_absoluteFolderPath, safeFileName);

        await using var output = File.Create(path);
        await fileStream.CopyToAsync(output);

        return $"/{RelativeFolder}/{safeFileName}";
    }

    /// <summary>
    /// Deletes an image from local development storage.
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public Task DeleteAsync(string fileName)
    {
        var safeName = Path.GetFileName(fileName);
        var fullPath = Path.Combine(_absoluteFolderPath, safeName);

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }

        return Task.CompletedTask;
    }
}
