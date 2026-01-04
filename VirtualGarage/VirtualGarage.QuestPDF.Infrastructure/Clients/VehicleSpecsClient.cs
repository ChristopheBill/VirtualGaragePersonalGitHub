using System;
using System.Net.Http.Json;
using VirtualGarage.Domain.Models;
using VirtualGarage.QuestPDF.Infrastructure.DTOs;
using VirtualGarage.QuestPDF.Infrastructure.Interfaces;

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

        return MapToDomain(dto);
    }

    private static VehicleSpecs MapToDomain(VehicleSpecsResponseDto dto)
    {
        return new VehicleSpecs
        {
            Brand = dto.Brand,
            Model = dto.Model,
            Year = dto.Year,
            Engine = dto.Engine,
            HorsePower = dto.HorsePower,
            FuelType = dto.FuelType,
            Transmission = dto.Transmission,
            Doors = dto.Doors,
            Seats = dto.Seats,
            DriveType = dto.DriveType
        };
    }
}