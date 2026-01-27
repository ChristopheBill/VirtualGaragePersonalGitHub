using System;

namespace VirtualGarage.Domain.Services.Interfaces;

public interface IBlobStorage
{
    Task<bool> ExistsAsync(string fileName);
    Task<string> UploadAsync(string fileName, byte[] content, string contentType);
    Task<byte[]> DownloadAsync(string fileName);
    string GetBlobUrl(string fileName);
    Task<byte[]> GetBlobBytesAsync(string fileName);
    Task<List<BlobInfo>> ListAllAsync();
}

public class BlobInfo
{
    public string Name { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public DateTimeOffset? CreatedOn { get; set; }
    public long? SizeInBytes { get; set; }
}