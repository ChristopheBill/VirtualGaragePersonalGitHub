using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualGarage.Persistence.Entities;
using VirtualGarage.Persistence.Interfaces;

namespace VirtualGarage.Persistence
{
    public class VehicleRepository(DbContexts.VirtualGarageDbContext dbContext) : IVehicleRepository
    {
        public async Task <Vehicle> CreateVehicleAsync(Entities.Vehicle vehicle)
        {
            dbContext.Vehicles.Add(vehicle);
            await dbContext.SaveChangesAsync();
            return vehicle;
        }
        public async Task <Vehicle?> GetVehicleByIdAsync(Guid id)
        {
            return await dbContext.Vehicles
                // .Include(v => v.ServiceRecords)
                .FirstOrDefaultAsync(v => v.Id == id);
        }
        public async Task <List<Entities.Vehicle>> GetAllVehiclesAsync()
        {
            return await dbContext.Vehicles
                // .Include(v => v.ServiceRecords)
                .ToListAsync();
        }
        public Task UpdateVehicleAsync(Entities.Vehicle vehicle)
        {
            dbContext.Vehicles.Update(vehicle);
            return dbContext.SaveChangesAsync();
        }
        public Task DeleteVehicleAsync(Guid id)
        {
            var vehicle = dbContext.Vehicles.Find(id);
            if (vehicle != null)
            {
                dbContext.Vehicles.Remove(vehicle);
            }
            return dbContext.SaveChangesAsync();
        }
    }
}
