using Azure.Storage.Blobs;
using VirtualGarage.Domain.Services.Interfaces;

public sealed class AzureBlobStorage : IBlobStorage
{
    private readonly BlobContainerClient _container;

    public AzureBlobStorage(BlobServiceClient client)
    {
        _container = client.GetBlobContainerClient("reports");
        _container.CreateIfNotExists();
    }

    public async Task<string> UploadAsync(
        string fileName,
        byte[] content,
        string contentType)
    {
        var blob = _container.GetBlobClient(fileName);

        using var stream = new MemoryStream(content);
        await blob.UploadAsync(stream, overwrite: true);

        return blob.Uri.ToString();
    }

    public async Task<byte[]> DownloadAsync(string fileName)
    {
        var blob = _container.GetBlobClient(fileName);
        var response = await blob.DownloadContentAsync();
        return response.Value.Content.ToArray();
    }
}