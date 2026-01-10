using System;

namespace VirtualGarage.Domain.Models;

public sealed class VehicleSpecs
{
    public string? Brand { get; init; } = default!;
    public string? Model { get; init; } = default!;
    public int? Year { get; init; }

    public string? Engine { get; init; } = default!;
    public int? HorsePower { get; init; }
    public string? FuelType { get; init; } = default!;

    public string? Transmission { get; init; } = default!;
    public string? DriveType { get; init; } = default!;

    public int? Doors { get; init; }
    public int? Seats { get; init; }
}