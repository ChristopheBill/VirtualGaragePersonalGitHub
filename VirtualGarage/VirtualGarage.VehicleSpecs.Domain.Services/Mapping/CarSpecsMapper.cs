using VirtualGarage.VehicleSpecs.Api.Contracts.ResponseContracts;
using VirtualGarage.VehicleSpecs.Infrastructure.CarApi.Contracts;
using VirtualGarage.VehicleSpecs.Persistence.Entities;
using System;

namespace VirtualGarage.VehicleSpecs.Domain.Services.Mapping;

public static class CarSpecsMapper
{
    public static CarSpecsResponse ToCarSpecsResponse(this CarApiCarResponse car)
        => new CarSpecsResponse(
            Brand: car.Brand,
            Model: car.Model,
            Year: car.Year,
            Engine: car.Engine?.Type,
            HorsePower: car.Engine?.Horsepower,
            FuelType: car.Engine?.Fuel,
            Transmission: car.Transmission,
            Doors: car.Doors,
            Seats: car.Seats,
            DriveType: car.DriveType
        );

    public static CarSpecs ToDomain(this CarApiCarResponse car, string id)
        => new CarSpecs
        {
            Id = id,
            Make = car.Brand,
            Model = car.Model,
            Year = car.Year,
            EngineType = car.Engine?.Type,
            FuelType = car.Engine?.Fuel,
            Horsepower = car.Engine?.Horsepower,
            Transmission = car.Transmission,
            DriveType = car.DriveType,
            Doors = car.Doors,
            Seats = car.Seats,
            RetrievedAt = DateTime.UtcNow,
            Source = "CarAPI"
        };

    public static CarSpecsResponse ToCarSpecsResponse(this CarSpecs specs)
        => new CarSpecsResponse(
            Brand: specs.Make,
            Model: specs.Model,
            Year: specs.Year,
            Engine: specs.EngineType,
            HorsePower: specs.Horsepower,
            FuelType: specs.FuelType,
            Transmission: specs.Transmission,
            Doors: specs.Doors,
            Seats: specs.Seats,
            DriveType: specs.DriveType
        );
}
