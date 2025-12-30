using System;

namespace VirtualGarage.VehicleSpecs.Api.Contracts.ResponseContracts;

public sealed record CarSpecsResponse(
    string Make,
    string Model,
    int Year,

    string? Engine,
    int? HorsePower,
    string? FuelType,
    string? Transmission,

    int? Doors,
    int? Seats,
    string? DriveType
);