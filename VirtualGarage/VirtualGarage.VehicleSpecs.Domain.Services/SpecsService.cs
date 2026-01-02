using System.Net.Http.Headers;
using VirtualGarage.VehicleSpecs.Api.Contracts.RequestContracts;
using VirtualGarage.VehicleSpecs.Api.Contracts.ResponseContracts;
using VirtualGarage.VehicleSpecs.Domain.Services.Interfaces;
using VirtualGarage.VehicleSpecs.Persistence.Entities;
using VirtualGarage.VehicleSpecs.Infrastructure.Interfaces;
using VirtualGarage.VehicleSpecs.Domain.Services.Mapping;

namespace VirtualGarage.VehicleSpecs.Domain.Services;

public sealed class SpecsService : ISpecsService
{
    private readonly ICarApiClient _carApi;

    public SpecsService(ICarApiClient carApi)
    {
        _carApi = carApi;
    }

    public async Task<CarSpecsResponse> LookupAsync(CarSpecsLookupRequest request)
    {
        var car = await _carApi.GetCarAsync(request.Brand, request.Model, request.Year);

        return car.ToCarSpecsResponse();
    }

    public async Task<CarSpecsResponse> GetSpecsAsync(string brand, string model, int year)
    {
        var car = await _carApi.GetCarAsync(brand, model, year);

        return car.ToCarSpecsResponse();
    }
}