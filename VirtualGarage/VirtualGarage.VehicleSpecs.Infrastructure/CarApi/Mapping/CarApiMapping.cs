using System;

namespace VirtualGarage.VehicleSpecs.Infrastructure.CarApi.Mapping;

using System.Text.Json;
using VirtualGarage.VehicleSpecs.Infrastructure.CarApi.Contracts;
using VirtualGarage.VehicleSpecs.Infrastructure.DTOs;
using VirtualGarage.VehicleSpecs.Persistence.Entities;

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

        var engine = trim.Engines?.FirstOrDefault();
        var body = trim.Bodies?.FirstOrDefault();
        var transmission = trim.Transmissions?.FirstOrDefault();
        var drive = trim.Drive_Types?.FirstOrDefault();

    return new CarApiCarResponse(
        Brand: trim.Make,
        Model: trim.Model,
        Year: trim.Year,
        Engine: engine != null
            ? new CarApiEngine(
                Type: engine.Engine_Type,
                Horsepower: engine.Horsepower_Hp,
                Fuel: engine.Fuel_Type
              )
            : null,
        Transmission: transmission?.Description,
        Doors: body?.Doors,
        Seats: body?.Seats,
        DriveType: drive?.Description
        );
    }
    // public static CarSpecs Map(CarApiTrimDetailResponse trim)
    // {  
    //     var engine = trim.Engines?.FirstOrDefault();
    //     var body = trim.Bodies?.FirstOrDefault();

    //     return new CarSpecs
    //         {
    //         Id = $"{trim.Make}-{trim.Model}-{trim.Year}".ToLower().ToString(),
    //         Make = trim.Make,
    //         Model = trim.Model,
    //         Year = trim.Year,

    //         EngineType = engine?.EngineType,
    //         FuelType = engine?.FuelType,
    //         Horsepower = engine?.HorsepowerHp,
    //         Torque = engine?.TorqueFtLbs,

    //         Transmission = trim.Transmissions?.FirstOrDefault()?.Description,
    //         DriveType = trim.DriveTypes?.FirstOrDefault()?.Description,

    //         Doors = body?.Doors,
    //         Seats = body?.Seats,

    //         RetrievedAt = DateTime.UtcNow,
    //     Source = "CarAPI"
    // };
}
