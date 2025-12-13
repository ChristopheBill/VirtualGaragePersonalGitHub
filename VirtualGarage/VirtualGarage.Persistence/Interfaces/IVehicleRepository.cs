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
        Vehicle CreateVehicle(Entities.Vehicle vehicle);
        Vehicle? GetVehicleById(Guid id);
        List<Vehicle> GetAllVehicles();
        void UpdateVehicle(Vehicle vehicle);
        void DeleteVehicle(int id);
    }
}
