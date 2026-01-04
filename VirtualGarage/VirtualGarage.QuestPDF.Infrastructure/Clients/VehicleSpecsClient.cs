using System;
using System.Net.Http.Json;
using VirtualGarage.Domain.Models;
using VirtualGarage.QuestPDF.Infrastructure.DTOs;
using VirtualGarage.QuestPDF.Infrastructure.Interfaces;
using VirtualGarage.QuestPDF.Infrastructure.Mapping;

namespace VirtualGarage.QuestPDF.Infrastructure.Clients;

public sealed class VehicleSpecsClient : IVehicleSpecsProvider
{
    private readonly HttpClient _httpClient;

    public VehicleSpecsClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<VehicleSpecs> GetSpecsAsync(
        string brand,
        string model,
        int year)
    {
        var url =
            $"api/vehiclespecs?" +
            $"brand={brand}&model={model}&year={year}";

        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            throw new ApplicationException(
                $"VehicleSpecs API failed: {response.StatusCode}");
        }

        var dto = await response.Content
            .ReadFromJsonAsync<VehicleSpecsResponseDto>();

        if (dto is null)
            throw new ApplicationException("Empty VehicleSpecs response");

        return VehicleSpecsMapper.MapToDomain(dto);
    }

    
}