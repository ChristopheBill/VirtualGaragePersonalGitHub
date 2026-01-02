using System;
using VirtualGarage.VehicleSpecs.Persistence.Entities;

namespace VirtualGarage.VehicleSpecs.Persistence.Interfaces;

public interface IVehicleSpecsRepository
{
    public Task<CarSpecs?> GetAsync(string id);
    public Task SaveAsync(CarSpecs specs);
}