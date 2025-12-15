using System;
using System.Collections.Generic;
using System.Text;
using VirtualGarage.Persistence.Entities;

namespace VirtualGarage.Domain.Services.Interfaces
{
    public interface IVehicleService
    {
        Task<Vehicle> GetVehicleAsync(Guid vehicleId);
        Task<Vehicle> CreateVehicleAsync(Vehicle vehicle);
        Task<List<Vehicle>> GetAllVehiclesAsync();
        Task UpdateVehicleAsync(Vehicle vehicle);
        Task DeleteVehicleAsync(Guid vehicleId);
    }
}
