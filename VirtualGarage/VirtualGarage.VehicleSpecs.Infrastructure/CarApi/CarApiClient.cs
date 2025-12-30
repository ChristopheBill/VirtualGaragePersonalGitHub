using System;
using System.Net.Http.Json;
using VirtualGarage.VehicleSpecs.Infrastructure.CarApi.Contracts;
using VirtualGarage.VehicleSpecs.Infrastructure.CarApi.Mapping;
using VirtualGarage.VehicleSpecs.Infrastructure.Interfaces;

namespace VirtualGarage.VehicleSpecs.Infrastructure.CarApi;

public sealed class CarApiClient : ICarApiClient
{
    private readonly HttpClient _httpClient;

    public CarApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<CarApiCarResponse> GetCarAsync(
        string make,
        string model,
        int year)
    {
        var response = await _httpClient.GetAsync(
            $"trims?make={Uri.EscapeDataString(make)}" +
            $"&model={Uri.EscapeDataString(model)}" +
            $"&year={year}");

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();

        return CarApiMapping.Map(json);
    }
}