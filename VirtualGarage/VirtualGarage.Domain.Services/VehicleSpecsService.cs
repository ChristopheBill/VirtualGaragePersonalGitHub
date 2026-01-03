using System;
using System.Net.Http.Json;
using VirtualGarage.Api.Contracts.ResponseContracts;
using VirtualGarage.Domain.Services.Mapping;
using VirtualGarage.Domain.Models;

namespace VirtualGarage.Domain.Services;

public sealed class VehicleSpecsService
{
    private readonly HttpClient _http;

    public VehicleSpecsService(HttpClient http)
    {
        _http = http;
    }

    public async Task<VehicleSpecsModel> GetAsync(
        string brand,
        string model,
        int year)
    {
        var response = await _http.GetFromJsonAsync<VehicleSpecsResponseContract>(
            $"vehiclespecs?brand={brand}&model={model}&year={year}");

        return response!.ToDomain();
    }
}