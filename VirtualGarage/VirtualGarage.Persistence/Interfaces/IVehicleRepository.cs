using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualGarage.Persistence.Entities;

namespace VirtualGarage.Persistence.Interfaces
{
    public interface IVehicleRepository
    {
        public Task<Vehicle> CreateVehicleAsync(Vehicle vehicle);
        public Task<Vehicle?> GetVehicleByIdAsync(Guid id);
        public Task<List<Vehicle>> GetAllVehiclesAsync();
        public Task UpdateVehicleAsync(Vehicle vehicle);
        public Task DeleteVehicleAsync(Guid id);
    }
}
