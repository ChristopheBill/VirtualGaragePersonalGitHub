using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualGarage.Persistence.Entities;

namespace VirtualGarage.Persistence.DbContexts
{
    public sealed class VirtualGarageDbContext(DbContextOptions<VirtualGarageDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
    }
}
