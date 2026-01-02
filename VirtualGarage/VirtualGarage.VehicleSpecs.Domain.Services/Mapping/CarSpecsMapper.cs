using VirtualGarage.VehicleSpecs.Api.Contracts.ResponseContracts;
using VirtualGarage.VehicleSpecs.Infrastructure.CarApi.Contracts;

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
}
