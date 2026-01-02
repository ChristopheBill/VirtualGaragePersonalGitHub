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

    public SpecsService(ICarApiClient carApi, IVehicleSpecsRepository repository)
    {
        _carApi = carApi;
        _repository = repository;
    }

    public async Task<CarSpecsResponse> LookupAsync(CarSpecsLookupRequest request)
    {
        var id = $"{request.Brand}-{request.Model}-{request.Year}".ToLower();

        var existing = await _repository.GetAsync(id);
        if (existing != null)
            return existing.ToCarSpecsResponse();

        var car = await _carApi.GetCarAsync(request.Brand, request.Model, request.Year);
        var specs = car.ToDomain(id);
        await _repository.SaveAsync(specs);

        return specs.ToCarSpecsResponse();
    }

    public async Task<CarSpecsResponse> GetSpecsAsync(string brand, string model, int year)
    {
        var id = $"{brand}-{model}-{year}".ToLower();

        var existing = await _repository.GetAsync(id);
        if (existing != null)
            return existing.ToCarSpecsResponse();

        var car = await _carApi.GetCarAsync(brand, model, year);
        var specs = car.ToDomain(id);
        await _repository.SaveAsync(specs);

        return specs.ToCarSpecsResponse();
    }
//     public async Task<CarSpecsResponse> GetAsync(string brand, string model, int year)
// {
//     var id = $"{brand}-{model}-{year}".ToLower();

//     var existing = await _repository.GetAsync(id);
//     if (existing != null)
//         return Map(existing);

//     var apiResult = await _carApi.GetCarAsync(brand, model, year);
//     var specs = MapToDomain(apiResult, id);

//     await _repository.SaveAsync(specs);

//     return Map(specs);
// }

    // private CarSpecsResponse Map(CarSpecs specs)
    // {
    //     return new CarSpecsResponse(
    //         specs.Make,
    //         specs.Model,
    //         specs.Year,
    //         specs.Horsepower?.ToString(),
    //         specs.Torque,
    //         specs.
    //     );
    // }

    // private CarSpecs MapToDomain(CarApiResponse apiResponse, string id)
    // {
    //     return new CarSpecs
    //     {
    //         Id = id,
    //         Make = apiResponse.Make,
    //         Model = apiResponse.Model,
    //         Year = apiResponse.Year,
    //         Horsepower = apiResponse.Horsepower,
    //         Torque = apiResponse.Torque,
    //         Weight = apiResponse.Weight
    //     };
    // }
}