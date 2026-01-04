using System;
using VirtualGarage.Domain.Models;

namespace VirtualGarage.Domain.Services.Interfaces;

public interface IVehicleSpecsProvider
{
    Task<VehicleSpecs> GetSpecsAsync(
        string brand,
        string model,
        int year);
}