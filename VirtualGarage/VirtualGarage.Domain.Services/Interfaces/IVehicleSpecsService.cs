using System;
using VirtualGarage.Domain.Models;

namespace VirtualGarage.Domain.Services.Interfaces;

public interface IVehicleSpecsService
{
    Task<VehicleSpecsModel> GetAsync(
        string brand,
        string model,
        int year);
}
