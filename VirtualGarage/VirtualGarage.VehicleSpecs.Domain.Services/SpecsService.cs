using System.Net.Http.Headers;
using VirtualGarage.VehicleSpecs.Api.Contracts.RequestContracts;
using VirtualGarage.VehicleSpecs.Api.Contracts.ResponseContracts;
using VirtualGarage.VehicleSpecs.Domain.Services.Interfaces;
using VirtualGarage.VehicleSpecs.Persistence.Entities;
using VirtualGarage.VehicleSpecs.Infrastructure.Interfaces;
using VirtualGarage.VehicleSpecs.Domain.Services.Mapping;
using VirtualGarage.VehicleSpecs.Persistence.Interfaces;
using VirtualGarage.VehicleSpecs.Infrastructure.DTOs;

namespace VirtualGarage.VehicleSpecs.Domain.Services;

public sealed class SpecsService : ISpecsService
{
    private readonly ICarApiClient _carApi;
    private readonly IVehicleSpecsRepository _repository;

    public SpecsService(
        ICarApiClient carApi,
        IVehicleSpecsRepository repository)
    {
        _carApi = carApi;
        _repository = repository;
    }

    // public Task<CarSpecsResponse> GetSpecsAsync(string brand, string model, int year)
    //     => GetOrFetchAsync(brand, model, year);

    public async Task<CarSpecsResponse> GetOrFetchAsync(
        string brand,
        string model,
        int year)
    {
        var id = BuildId(brand, model, year);

        var existing = await _repository.GetAsync(id);
        if (existing != null)
            return existing.ToCarSpecsResponse();

        var car = await _carApi.GetCarAsync(brand, model, year);

        var specs = car.ToDomain(id);
        await _repository.SaveAsync(specs);

        return specs.ToCarSpecsResponse();
    }

    private static string BuildId(string brand, string model, int year)
        => $"{brand}-{model}-{year}".ToLowerInvariant();
}