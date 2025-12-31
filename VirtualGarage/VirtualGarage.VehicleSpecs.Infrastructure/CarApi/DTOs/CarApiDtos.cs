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

public record CarApiTrimDetailResponse
{
    public int Id { get; set; }
    public string Make { get; set; } = null!;
    public string Model { get; set; } = null!;
    public int Year { get; set; }

    public List<CarApiEngineDto>? Engines { get; set; }
    public List<CarApiBodyDto>? Bodies { get; set; }
    public List<CarApiTransmissionDto>? Transmissions { get; set; }
    public List<CarApiDriveTypeDto>? Drive_Types { get; set; }

    // Optional: you can add colors, mileages, etc. later
}

public class CarApiEngineDto
{
    public string? Engine_Type { get; set; }
    public string? Fuel_Type { get; set; }
    public int? Horsepower_Hp { get; set; }
}

public class CarApiBodyDto
{
    public int? Doors { get; set; }
    public int? Seats { get; set; }
}

public class CarApiTransmissionDto
{
    public string? Description { get; set; }
}

public class CarApiDriveTypeDto
{
    public string? Description { get; set; }
}

