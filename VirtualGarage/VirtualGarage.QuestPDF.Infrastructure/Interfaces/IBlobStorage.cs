using System;

namespace VirtualGarage.Domain.Services.Interfaces;

public interface IBlobStorage
{
    Task<bool> ExistsAsync(string fileName);
    Task<string> UploadAsync(string fileName, byte[] content, string contentType);
    Task<byte[]> DownloadAsync(string fileName);
    string GetBlobUrl(string fileName);
    Task<byte[]> GetBlobBytesAsync(string fileName);
}