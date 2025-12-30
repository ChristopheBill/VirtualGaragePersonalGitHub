using System;

namespace VirtualGarage.VehicleSpecs.Infrastructure.DTOs;

public sealed class CarApiTrimsResponse
{
    public List<CarApiTrimDto> data { get; set; } = [];
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
public sealed class CarApiTrimDto
{
    public int id { get; set; }

    public string make { get; set; } = default!;
    public string model { get; set; } = default!;
    public int year { get; set; }

    public string? name { get; set; } // trim name

    public CarApiEngineDto engine { get; set; } = default!;

    public string? transmission { get; set; }
    public string? drive { get; set; }

    public int? doors { get; set; }
    public int? seats { get; set; }
}
public sealed class CarApiEngineDto
{
    public string? type { get; set; }
    public int? horsepower { get; set; }
    public string? fuel { get; set; }
}
