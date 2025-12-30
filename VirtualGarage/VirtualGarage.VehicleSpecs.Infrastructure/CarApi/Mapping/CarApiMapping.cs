using System;

namespace VirtualGarage.VehicleSpecs.Infrastructure.CarApi.Mapping;

using System.Text.Json;
using VirtualGarage.VehicleSpecs.Infrastructure.CarApi.Contracts;
using VirtualGarage.VehicleSpecs.Infrastructure.DTOs;

public static class CarApiMapping
{
    public static CarApiCarResponse Map(string json)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var response =
            JsonSerializer.Deserialize<CarApiTrimsResponse>(json, options)
            ?? throw new InvalidOperationException("Invalid CarAPI response");

        var trim = PickBestTrim(response.data);

        return new CarApiCarResponse(
            Make: trim.make,
            Model: trim.model,
            Year: trim.year,
            Engine: new CarApiEngine(
                Type: trim.engine?.type,
                Horsepower: trim.engine?.horsepower,
                Fuel: trim.engine?.fuel
            ),
            Transmission: trim.transmission,
            Doors: trim.doors,
            Seats: trim.seats,
            DriveType: trim.drive
        );
    }

    private static CarApiTrimDto PickBestTrim(
        List<CarApiTrimDto> trims)
    {
        if (trims.Count == 0)
            throw new InvalidOperationException("No trims found");

        // Simple rule for now (exam-friendly)
        return trims
            .OrderByDescending(t => t.engine?.horsepower ?? 0)
            .First();
    }
}