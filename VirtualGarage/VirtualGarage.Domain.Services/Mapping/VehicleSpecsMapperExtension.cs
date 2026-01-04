using System;
using VirtualGarage.Api.Contracts.ResponseContracts;
using VirtualGarage.Domain.Models;

namespace VirtualGarage.Domain.Services.Mapping;

internal static class VehicleSpecsMapperExtension
{
    public static VehicleSpecs ToDomain(this VehicleSpecsResponseContract r)
        => new()
        {
            Brand = r.Brand,
            Model = r.Model,
            Year = r.Year,
            Engine = r.Engine,
            HorsePower = r.HorsePower,
            FuelType = r.FuelType,
            Transmission = r.Transmission,
            DriveType = r.DriveType,
            Doors = r.Doors,
            Seats = r.Seats
        };
}

