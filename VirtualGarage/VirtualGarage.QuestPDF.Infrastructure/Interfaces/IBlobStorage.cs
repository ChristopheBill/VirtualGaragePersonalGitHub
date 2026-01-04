using System;

namespace VirtualGarage.Domain.Services.Interfaces;

public interface IBlobStorage
{
    Task<string> UploadAsync(
        string fileName,
        byte[] content,
        string contentType);

    Task<byte[]> DownloadAsync(string fileName);
}