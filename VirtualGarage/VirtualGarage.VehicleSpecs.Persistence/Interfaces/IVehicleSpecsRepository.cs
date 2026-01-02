using System;
using VirtualGarage.VehicleSpecs.Persistence.Entities;

namespace VirtualGarage.VehicleSpecs.Persistence.Interfaces;

public interface IVehicleSpecsRepository
{
    Task<CarSpecs?> GetAsync(string id);
    Task SaveAsync(CarSpecs specs);
}