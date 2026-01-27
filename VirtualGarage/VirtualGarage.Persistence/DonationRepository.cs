using Microsoft.EntityFrameworkCore;
using VirtualGarage.Persistence.DbContexts;
using VirtualGarage.Persistence.Entities;
using VirtualGarage.Persistence.Interfaces;

namespace VirtualGarage.Persistence
{
    public class DonationRepository(VirtualGarageDbContext context) : IDonationRepository
    {
        public async Task CreateDonationAsync(Donation donation)
        {
            await context.Donations.AddAsync(donation);
            await context.SaveChangesAsync();
        }

        public async Task<Donation?> GetDonationByIdAsync(Guid donationId)
        {
            return await context.Donations.FirstOrDefaultAsync(d => d.Id == donationId);
        }

        public async Task<List<Donation>> GetDonationsByUserIdAsync(Guid userId)
        {
            return await context.Donations
                .Where(d => d.UserId == userId)
                .OrderByDescending(d => d.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Donation>> GetAllDonationsAsync()
        {
            return await context.Donations.OrderByDescending(d => d.CreatedAt).ToListAsync();
        }
    }
}
