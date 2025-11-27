using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualGarage.Persistence.Entities;

namespace VirtualGarage.Persistence.DbContexts
{
    public class VirtualGarageDbContext : DbContext
    {
        public VirtualGarageDbContext(DbContextOptions<VirtualGarageDbContext> options)
            : base(options)
        {
        }
        public DbSet <User> Users { get; set; }
        public DbSet <Vehicle> Vehicles { get; set; }
    }
}
