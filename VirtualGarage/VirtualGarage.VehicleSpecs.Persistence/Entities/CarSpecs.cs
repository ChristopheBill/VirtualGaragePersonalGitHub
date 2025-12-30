using System;

namespace VirtualGarage.VehicleSpecs.Persistence.Entities;

public class CarSpecs
{
    public Guid Id { get; set; }

    // Link to your Vehicle
    public Guid VehicleId { get; set; }

    // Identity
    public string Make { get; set; } = default!;
    public string Model { get; set; } = default!;
    public int Year { get; set; }

    // Engine
    public string? Engine { get; set; }
    public int? Horsepower { get; set; }
    public int? Torque { get; set; }

    // Drivetrain
    public string? Transmission { get; set; }
    public string? DriveType { get; set; }

    // Dimensions
    public int? Doors { get; set; }
    public int? Seats { get; set; }

    // Meta
    public DateTime RetrievedAt { get; set; }
}
