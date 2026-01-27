using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using VirtualGarage.Domain.Services.Interfaces;

public sealed class AzureBlobStorage : IBlobStorage
{
    private readonly BlobContainerClient _container;

    public AzureBlobStorage(BlobServiceClient client)
    {
        _container = client.GetBlobContainerClient("virtualgarageblob");
        _container.CreateIfNotExists();
    }

    public async Task<bool> ExistsAsync(string fileName)
    {
        var blob = _container.GetBlobClient(fileName);
        return await blob.ExistsAsync();
    }

    public async Task<string> UploadAsync(string fileName, byte[] content, string contentType)
    {
        var blob = _container.GetBlobClient(fileName);

        using var stream = new MemoryStream(content);
        await blob.UploadAsync(stream, new BlobUploadOptions
        {
            HttpHeaders = new BlobHttpHeaders
            {
                ContentType = contentType
            }
        }
        );
        return blob.Uri.ToString();
    }

    public async Task<byte[]> DownloadAsync(string fileName)
    {
        var blob = _container.GetBlobClient(fileName);
        var response = await blob.DownloadContentAsync();
        return response.Value.Content.ToArray();
    }

    public string GetBlobUrl(string fileName)
    {
        var blob = _container.GetBlobClient(fileName);
        return blob.Uri.ToString();
    }
    public async Task<byte[]> GetBlobBytesAsync(string fileName)
    {
        var blob = _container.GetBlobClient(fileName);
        var response = await blob.DownloadContentAsync();
        return response.Value.Content.ToArray();
    }

    public async Task<List<VirtualGarage.Domain.Services.Interfaces.BlobInfo>> ListAllAsync()
    {
        var blobs = new List<VirtualGarage.Domain.Services.Interfaces.BlobInfo>();
        
        await foreach (var blob in _container.GetBlobsAsync())
        {
            blobs.Add(new VirtualGarage.Domain.Services.Interfaces.BlobInfo
            {
                Name = blob.Name,
                Url = _container.GetBlobClient(blob.Name).Uri.ToString(),
                CreatedOn = blob.Properties.CreatedOn,
                SizeInBytes = blob.Properties.ContentLength
            });
        }
        
        return blobs;
    }
}