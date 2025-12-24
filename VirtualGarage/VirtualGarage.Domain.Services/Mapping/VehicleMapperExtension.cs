using System;

namespace VirtualGarage.Domain.Services.Mapping;

internal static class VehicleMapperExtension
{
    public static Persistence.Entities.Vehicle ToEntity(this Api.Contracts.VehicleRequestContract vehicleContract)
    {
        return new Persistence.Entities.Vehicle
        {
            Id = Guid.NewGuid(),
            UserId = vehicleContract.OwnerId,
            Brand = vehicleContract.Make,
            Model = vehicleContract.Model,
            ManufactureDate = vehicleContract.Year,
        };
    }
}
