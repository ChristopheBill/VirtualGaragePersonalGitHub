using System;

namespace VirtualGarage.VehicleSpecs.Infrastructure.CarApi.Contracts;

public sealed record CarApiCarResponse(
    string Make,
    string Model,
    int Year,
    CarApiEngine Engine,
    string? Transmission,
    int? Doors,
    int? Seats,
    string? DriveType
);

public sealed record CarApiEngine(
    string? Type,
    int? Horsepower,
    string? Fuel
);