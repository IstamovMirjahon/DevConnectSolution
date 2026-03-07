using DevConnect.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace DevConnect.Api.Services;

public class LocalFileStorageService(IConfiguration configuration, IWebHostEnvironment env) : IFileStorageService
{
    public async Task<string> SaveFileAsync(IFormFile file, CancellationToken ct = default)
    {
        var imagesFolder = Path.Combine(env.WebRootPath, "images");
        Directory.CreateDirectory(imagesFolder);

        var extension = Path.GetExtension(file.FileName);
        var fileName = $"{Guid.NewGuid()}{extension}";
        var filePath = Path.Combine(imagesFolder, fileName);

        await using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream, ct);

        var baseUrl = configuration["BaseUrl"]?.TrimEnd('/');
        return $"{baseUrl}/images/{fileName}";
    }
}
