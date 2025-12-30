using System;

namespace VirtualGarage.VehicleSpecs.Infrastructure.CarApi.Contracts;

public sealed record CarApiCarResponse(
    string ake,
    string Model,
    int Year,
    CarApiEngine Engine
    // ,
    // CarApiDimensions dimensions
);

public sealed record CarApiEngine(
    string Type,
    int Horsepower,
    string Fuel
);