using System;
using VirtualGarage.VehicleSpecs.Persistence.Entities;

namespace VirtualGarage.VehicleSpecs.Domain.Services.Mapping;

internal static class CarSpecsMappingExtension
{
    public static CarSpecs ToEntity(this object apiResponse)
    {
        // TODO: Implement mapping logic
        return new CarSpecs();
    }

    public static CarSpecs MapToCarSpecs(string json)
    {
        throw new NotImplementedException();
    }
}
