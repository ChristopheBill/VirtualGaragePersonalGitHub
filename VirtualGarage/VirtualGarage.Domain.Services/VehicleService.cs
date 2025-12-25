using System;
using System.Collections.Generic;
using System.Text;
using VirtualGarage.Api.Contracts;
using VirtualGarage.Domain.Services.Interfaces;
using VirtualGarage.Domain.Services.Mapping;
using VirtualGarage.Persistence.Entities;
using VirtualGarage.Persistence.Interfaces;

namespace VirtualGarage.Domain.Services
{
    public class VehicleService (IVehicleRepository vehicleRepository) : IVehicleService
    {
        public async Task<Vehicle?> GetVehicleAsync(Guid vehicleId)
        {
            var vehicle = await vehicleRepository.GetVehicleByIdAsync(vehicleId);
            return vehicle;
        }
        public async Task<Vehicle> CreateVehicleAsync(VehicleRequestContract vehicle)
        {
            var createdVehicle = await vehicleRepository.CreateVehicleAsync(vehicle.ToEntity());
            return createdVehicle;
        }
        public async Task<List<Vehicle>> GetAllVehiclesAsync()
        {
            var vehicles = await vehicleRepository.GetAllVehiclesAsync();
            return vehicles;
        }
        public async Task<Vehicle> UpdateVehicleAsync(Guid id, VehicleRequestContract vehicle)
        {
            await vehicleRepository.UpdateVehicleAsync(id, vehicle.ToEntity());
            return await GetVehicleAsync(id);
        }
        public async Task DeleteVehicleAsync(Guid vehicleId)
        {
            await vehicleRepository.DeleteVehicleAsync(vehicleId);
        }
        public async Task<List<Vehicle>> GetVehiclesForUserAsync(Guid userId)
        {
            var vehicles = await vehicleRepository.GetVehiclesByUserIdAsync(userId);
            return vehicles;
        }
    }
}
