using System;
using System.Text.Json.Serialization;

namespace VirtualGarage.QuestPDF.Infrastructure.Storage;

public sealed class VehicleSpecsDocument
{
    [JsonPropertyName("id")]
    public string? Id { get; set; } = default!;

    public string? Brand { get; set; } = default!;
    public string? Model { get; set; } = default!;
    public int? Year { get; set; }

    public string? Engine { get; set; }
    public int? HorsePower { get; set; }
    public string? FuelType { get; set; }

    public string? Transmission { get; set; }
    public int? Doors { get; set; }
    public int? Seats { get; set; }
    public string? DriveType { get; set; }

    public DateTime RetrievedAt { get; set; }
    public string Source { get; set; } = default!;
}