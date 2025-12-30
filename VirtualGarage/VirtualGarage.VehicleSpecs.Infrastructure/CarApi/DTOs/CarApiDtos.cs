using System;

namespace VirtualGarage.VehicleSpecs.Infrastructure.DTOs;

public class CarApiTrimResponse
{
    public List<CarApiTrim> Data { get; set; } = [];
}

public class CarApiTrim
{
    public string? Name { get; set; }
    public int? Horsepower { get; set; }
    public int? Torque { get; set; }
    public string? Transmission { get; set; }
    public string? Drive { get; set; }
    public int? Doors { get; set; }
    public int? Seats { get; set; }
}
