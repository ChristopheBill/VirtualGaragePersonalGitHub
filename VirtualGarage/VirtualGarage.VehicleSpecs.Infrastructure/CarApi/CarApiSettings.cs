using System;

namespace VirtualGarage.VehicleSpecs.Infrastructure.CarApi;

public class CarApiSettings
{
    public string BaseUrl { get; set; } = default!;
    public string JwtToken { get; set; } = default!;
}