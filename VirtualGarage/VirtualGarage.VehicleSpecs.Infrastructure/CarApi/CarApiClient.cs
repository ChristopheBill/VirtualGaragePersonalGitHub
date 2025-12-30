using System;
using System.Net.Http.Json;
using VirtualGarage.VehicleSpecs.Infrastructure.CarApi.Contracts;

namespace VirtualGarage.VehicleSpecs.Infrastructure.CarApi;

public sealed class CarApiClient
{
    private readonly HttpClient _http;

    public CarApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<CarApiCarResponse> GetCarAsync(
        string make,
        string model,
        int year)
    {
        var response = await _http.GetFromJsonAsync<CarApiCarResponse>(
            $"cars?make={make}&model={model}&year={year}");

        return response!;
    }
}