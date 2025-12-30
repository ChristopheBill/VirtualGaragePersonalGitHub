using System.Net.Http.Headers;
using VirtualGarage.VehicleSpecs.Api.Contracts.RequestContracts;
using VirtualGarage.VehicleSpecs.Api.Contracts.ResponseContracts;
using VirtualGarage.VehicleSpecs.Domain.Services.Interfaces;
using VirtualGarage.VehicleSpecs.Domain.Services.Mapping;
using VirtualGarage.VehicleSpecs.Persistence.Entities;
using VirtualGarage.VehicleSpecs.Infrastructure.Interfaces;

namespace VirtualGarage.VehicleSpecs.Domain.Services;

public sealed class SpecsService : ISpecsService
{
    private readonly ICarApiClient _carApi;

    public SpecsService(ICarApiClient carApi)
    {
        _carApi = carApi;
    }

    public async Task<CarSpecsResponse> LookupAsync(
        CarSpecsLookupRequest request)
    {
        var car = await _carApi.GetCarAsync(
            request.Make,
            request.Model,
            request.Year);

        return new CarSpecsResponse(
            Make: car.Make,
            Model: car.Model,
            Year: car.Year,
            Engine: car.Engine.Type,
            HorsePower: car.Engine.Horsepower,
            FuelType: car.Engine.Fuel,
            Transmission: car.Transmission,
            Doors: car.Doors,
            Seats: car.Seats,
            DriveType: car.DriveType
        );
    }
}