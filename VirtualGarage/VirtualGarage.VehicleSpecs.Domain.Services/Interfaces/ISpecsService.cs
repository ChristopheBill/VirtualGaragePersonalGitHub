using System;
using VirtualGarage.VehicleSpecs.Persistence.Entities;

namespace VirtualGarage.VehicleSpecs.Domain.Services.Interfaces;

public interface ISpecsService
{
    Task<CarSpecs> GetSpecsAsync(string brand, string model, int year);
}
