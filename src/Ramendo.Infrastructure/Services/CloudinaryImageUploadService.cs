using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Configuration;
using Ramendo.Application.Common;

namespace Ramendo.Infrastructure.Services;

public sealed class CloudinaryImageUploadService : IImageUploadService
{
    private readonly Cloudinary _cloudinary;

    public CloudinaryImageUploadService(IConfiguration config)
    {
        var cloudName = config["Cloudinary:CloudName"]!;
        var apiKey    = config["Cloudinary:ApiKey"]!;
        var apiSecret = config["Cloudinary:ApiSecret"]!;
        _cloudinary = new Cloudinary(new Account(cloudName, apiKey, apiSecret));
        _cloudinary.Api.Secure = true;
    }

    public async Task<string> UploadAsync(Stream stream, string fileName, string folder, CancellationToken ct = default)
    {
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(fileName, stream),
            Folder = folder,
            UseFilename = false,
            UniqueFilename = true,
            Overwrite = false,
        };
        var result = await _cloudinary.UploadAsync(uploadParams, ct);
        if (result.Error is not null)
            throw new InvalidOperationException($"Cloudinary upload failed: {result.Error.Message}");
        return result.SecureUrl.ToString();
    }

    public async Task DeleteAsync(string url, CancellationToken ct = default)
    {
        var publicId = ExtractPublicId(url);
        if (publicId is null) return;
        await _cloudinary.DestroyAsync(new DeletionParams(publicId));
    }

    private static string? ExtractPublicId(string url)
    {
        // e.g. https://res.cloudinary.com/<cloud>/image/upload/v123/<folder>/<id>.jpg
        var uri = new Uri(url);
        var segments = uri.AbsolutePath.Split('/');
        var uploadIdx = Array.IndexOf(segments, "upload");
        if (uploadIdx < 0 || uploadIdx + 2 >= segments.Length) return null;
        // skip version segment (v123)
        var start = segments[uploadIdx + 1].StartsWith('v') ? uploadIdx + 2 : uploadIdx + 1;
        var parts = segments[start..];
        var withoutExt = string.Join("/", parts);
        var dot = withoutExt.LastIndexOf('.');
        return dot >= 0 ? withoutExt[..dot] : withoutExt;
    }
}
