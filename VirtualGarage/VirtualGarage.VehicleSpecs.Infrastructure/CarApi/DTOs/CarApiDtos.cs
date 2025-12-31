using System;

namespace VirtualGarage.VehicleSpecs.Infrastructure.DTOs;

public sealed class CarApiResponse
{
    public CarApiCollection Collection { get; set; } = null!;
    public List<CarApiTrimDto> Data { get; set; } = new();
}

// DTO representing a car returned by CarAPI

public record CarApiTrimDto(
    int Id,
    int? MakeId,
    int? ModelId,
    int? SubmodelId,
    int Year,
    string Make,
    string Model,
    string? Series,
    string? Submodel,
    string? Trim,
    string? Description,
    decimal? MsRp,
    decimal? Invoice,
    string? Created,
    string? Modified
);

// The response for trims list endpoint
public record CarApiTrimsListResponse(
    CarApiCollection? Collection,
    List<CarApiTrimDto>? Data
);

public record CarApiCollection(
    string? Url,
    int Count,
    int Pages,
    int Total,
    string? Next,
    string? Prev,
    string? First,
    string? Last
);

// Detailed info for a single trim
public record CarApiTrimDetailResponse(
    string Make,
    string Model,
    int Year,
    CarApiEngineDto? Engine,
    string? Transmission,
    int? Doors,
    int? Seats,
    string? DriveType
);

// Engine info inside trim details
public record CarApiEngineDto(
    string? Type,
    int? Horsepower,
    string? Fuel
);