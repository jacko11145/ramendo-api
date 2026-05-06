namespace Ramendo.Application.Common;

public interface IImageUploadService
{
    Task<string> UploadAsync(Stream stream, string fileName, string folder, CancellationToken ct = default);
    Task DeleteAsync(string url, CancellationToken ct = default);
}
