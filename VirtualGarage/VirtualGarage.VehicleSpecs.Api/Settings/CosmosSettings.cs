using System;

namespace VirtualGarage.VehicleSpecs.Api.Settings;

public sealed record CosmosSettings
{
    public string DatabaseName { get; init; } = "VirtualGarage";
    public string ContainerName { get; init; } = "VehicleSpecs";
    public string? ConnectionString { get; init; }
    public string? AccountEndpoint { get; init; }
    public string? AccountKey { get; init; }
}