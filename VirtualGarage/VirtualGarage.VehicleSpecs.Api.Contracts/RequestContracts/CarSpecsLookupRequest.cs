using System;

namespace VirtualGarage.VehicleSpecs.Api.Contracts.RequestContracts;

public sealed record CarSpecsLookupRequest(
    string Brand,
    string Model,
    int Year
);