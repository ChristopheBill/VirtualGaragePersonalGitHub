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

    // Constructor receives an HttpClient that's configured with BaseAddress
    public VehicleSpecsClient(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task<VehicleSpecs> GetSpecsAsync(string brand, string model, int year)
    {
        if (string.IsNullOrWhiteSpace(brand)) throw new ArgumentException("Brand is required", nameof(brand));
        if (string.IsNullOrWhiteSpace(model)) throw new ArgumentException("Model is required", nameof(model));

        var url = $"api/specifications?brand={Uri.EscapeDataString(brand)}&model={Uri.EscapeDataString(model)}&year={year}";

        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            throw new ApplicationException($"VehicleSpecs API failed: {response.StatusCode}");
        }

        var dto = await response.Content.ReadFromJsonAsync<VehicleSpecsResponseDto>();

        if (dto is null)
            throw new ApplicationException("Empty VehicleSpecs response");

        return VehicleSpecsMapper.MapToDomain(dto);
    }
}