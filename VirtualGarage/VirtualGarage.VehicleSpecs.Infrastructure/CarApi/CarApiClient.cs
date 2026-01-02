using System.Net.Http.Json;
using VirtualGarage.VehicleSpecs.Infrastructure.CarApi.Contracts;
using VirtualGarage.VehicleSpecs.Infrastructure.CarApi.Mapping;
using VirtualGarage.VehicleSpecs.Infrastructure.DTOs;
using VirtualGarage.VehicleSpecs.Infrastructure.Interfaces;

namespace VirtualGarage.VehicleSpecs.Infrastructure.CarApi;

public sealed class CarApiClient : ICarApiClient
{
    private readonly HttpClient _httpClient;

    public CarApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<CarApiCarResponse> GetCarAsync(string brand, string model, int year)
{
    // 1️ Get the list of trims
    var trimsResponse = await _httpClient.GetFromJsonAsync<CarApiTrimsListResponse>(
        $"trims/v2?brand={Uri.EscapeDataString(brand)}&model={Uri.EscapeDataString(model)}&year={year}");

    if (trimsResponse == null || trimsResponse.Data == null || !trimsResponse.Data.Any())
        throw new InvalidOperationException($"No trims found for {brand} {model} {year}");

    // 2️ Pick the first trim
    var trim = trimsResponse.Data.First();

    // 3 Get full trim details
    var detailResponse = await _httpClient.GetFromJsonAsync<CarApiTrimDetailResponse>(
    $"trims/v2/{trim.Id}");
    var carSpecs = CarApiMapping.MapTrimDetail(detailResponse);
    return carSpecs;

}
}