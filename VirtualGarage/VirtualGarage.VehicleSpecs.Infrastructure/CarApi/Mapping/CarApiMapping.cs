using System;

namespace VirtualGarage.VehicleSpecs.Infrastructure.CarApi.Mapping;

using System.Text.Json;
using VirtualGarage.VehicleSpecs.Infrastructure.CarApi.Contracts;
using VirtualGarage.VehicleSpecs.Infrastructure.DTOs;

public static class CarApiMapping
{
    public static int PickTrimIdFromList(string json)
    {
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var response = JsonSerializer.Deserialize<CarApiTrimsListResponse>(json, options)
                       ?? throw new InvalidOperationException("Invalid CarAPI trims list response");

        if (response.Data == null || response.Data.Count == 0)
            throw new InvalidOperationException("No trims found in list");

        // Pick first trim (can improve later)
        return response.Data.First().Id;
    }

    // Step 2: Map from trim detail response to CarApiCarResponse
    public static CarApiCarResponse MapTrimDetail(CarApiTrimDetailResponse trim)
    {
        if (trim == null) throw new ArgumentNullException(nameof(trim));

        return new CarApiCarResponse(
            Brand: trim.Make,
            Model: trim.Model,
            Year: trim.Year,
            Engine: trim.Engine != null 
                ? new CarApiEngine(
                    Type: trim.Engine.Type,
                    Horsepower: trim.Engine.Horsepower,
                    Fuel: trim.Engine.Fuel
                )
                : null,
            Transmission: trim.Transmission,
            Doors: trim.Doors,
            Seats: trim.Seats,
            DriveType: trim.DriveType
        );
    }
}