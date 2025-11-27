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
        public Vehicle CreateVehicle(Entities.Vehicle vehicle)
        {
            dbContext.Vehicles.Add(vehicle);
            dbContext.SaveChanges();
            return vehicle;
        }
        public Entities.Vehicle? GetVehicleById(int id)
        {
            return dbContext.Vehicles
                .Include(v => v.ServiceRecords)
                .FirstOrDefault(v => v.Id == id);
        }
        public List<Entities.Vehicle> GetAllVehicles()
        {
            return dbContext.Vehicles
                .Include(v => v.ServiceRecords)
                .ToList();
        }
        public void UpdateVehicle(Entities.Vehicle vehicle)
        {
            dbContext.Vehicles.Update(vehicle);
            dbContext.SaveChanges();
        }
        public void DeleteVehicle(int id)
        {
            var vehicle = dbContext.Vehicles.Find(id);
            if (vehicle != null)
            {
                dbContext.Vehicles.Remove(vehicle);
                dbContext.SaveChanges();
            }
        }
    }
}
