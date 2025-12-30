using System;

namespace VirtualGarage.VehicleSpecs.Api.Contracts.RequestContracts;

public sealed record CarSpecsLookupRequest(
    string Make,
    string Model,
    int Year
);