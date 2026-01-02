using System;

namespace VirtualGarage.VehicleSpecs.Infrastructure.CarApi;

public class CarApiSettings
{
    public string BaseUrl { get; set; } = null!;
    public string JwtToken { get; set; } = null!;
}