using System;

namespace VirtualGarage.VehicleSpecs.Persistence.Entities;

public class CarSpecs
{
    // Cosmos DB required
    public string Id { get; set; } = default!;   // "toyota-corolla-2019"
    public string PartitionKey => Make.ToLowerInvariant();
    // Identity
    public string Make { get; set; } = default!;
    public string Model { get; set; } = default!;
    public int Year { get; set; }

    // Engine
    public string? EngineType { get; set; }
    public string? FuelType { get; set; }
    public int? Horsepower { get; set; }
    public int? Torque { get; set; }

    // Drivetrain
    public string? Transmission { get; set; }
    public string? DriveType { get; set; }

    // Dimensions
    public int? Doors { get; set; }
    public int? Seats { get; set; }

    // Metadata
    public DateTime RetrievedAt { get; set; }
    public string Source { get; set; } = "CarAPI";
}
