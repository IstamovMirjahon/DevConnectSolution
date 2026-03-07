using Microsoft.AspNetCore.Http;

namespace DevConnect.Application.Interfaces;

public interface IFileStorageService
{
    Task<string> SaveFileAsync(IFormFile file, CancellationToken ct = default);
}
