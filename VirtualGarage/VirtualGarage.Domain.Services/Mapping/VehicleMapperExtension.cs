using System;

namespace VirtualGarage.Domain.Services.Mapping;

internal static class VehicleMapperExtension
{
    public static Persistence.Entities.Vehicle ToEntity(this Api.Contracts.VehicleRequestContract vehicleContract)
    {
        return new Persistence.Entities.Vehicle
        {
            Id = Guid.NewGuid(),
            Brand = vehicleContract.Make,
            Model = vehicleContract.Model,
            ManufactureDate = vehicleContract.ManufactureDate,
        };
    }
}
