using System;

namespace VirtualGarage.QuestPDF.Infrastructure.DTOs;

public sealed class VehicleSpecsResponseDto
{
    public string? Brand { get; set; }
    public string? Model { get; set; }
    public int? Year { get; set; }
    public string? Engine { get; set; }
    public int? HorsePower { get; set; }
    public string? FuelType { get; set; }
    public string? Transmission { get; set; }
    public int? Doors { get; set; }
    public int? Seats { get; set; }
    public string? DriveType { get; set; }
}