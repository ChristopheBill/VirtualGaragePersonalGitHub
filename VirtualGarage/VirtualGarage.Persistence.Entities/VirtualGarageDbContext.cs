using Microsoft.EntityFrameworkCore;

namespace VirtualGarage.Persistence.Entities;

public class VirtualGarageDbContext : DbContext
{
    public DbSet<Donation> Donations { get; set; } = null!;
}