using System;
using System.Collections.Generic;
using System.Text;
using VirtualGarage.Api.Contracts;
using VirtualGarage.Persistence.Entities;

namespace VirtualGarage.Domain.Services.Interfaces
{
    public interface IVehicleService
    {
        Task<Vehicle?> GetVehicleAsync(Guid vehicleId);
        Task<Vehicle> CreateVehicleAsync(VehicleRequestContract vehicle, Guid currentUserId);
        Task<List<Vehicle>> GetAllVehiclesAsync();
        Task <Vehicle> UpdateVehicleAsync(Guid id, VehicleRequestContract vehicle);
        Task DeleteVehicleAsync(Guid vehicleId);
        Task<List<Vehicle>> GetVehiclesForUserAsync(Guid userId);
    }
}
