using System;

namespace VirtualGarage.VehicleSpecs.Infrastructure.CarApi.Contracts;

public sealed record CarApiCarResponse(
    string make,
    string model,
    int year,
    CarApiEngine engine
    // ,
    // CarApiDimensions dimensions
);

public sealed record CarApiEngine(
    string type,
    int horsepower,
    string fuel
);