using System;

namespace VirtualGarage.Infrastructure.Storage;

public class BlobStorageOptions
{
    public string ConnectionString { get; set; } = string.Empty;
    public string ContainerName { get; set; } = string.Empty;
}
