using System;
using VirtualGarage.VehicleSpecs.Persistence.Entities;

namespace VirtualGarage.VehicleSpecs.Domain.Services.Interfaces;

public interface ISpecService
{
    Task<Specs> GetSpecsAsync(string brand, string model, int year);
}
