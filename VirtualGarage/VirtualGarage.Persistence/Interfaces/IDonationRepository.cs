using VirtualGarage.Persistence.Entities;

namespace VirtualGarage.Persistence.Interfaces
{
    public interface IDonationRepository
    {
        Task CreateDonationAsync(Donation donation);
        Task<Donation?> GetDonationByIdAsync(Guid donationId);
        Task<List<Donation>> GetDonationsByUserIdAsync(Guid userId);
        Task<List<Donation>> GetAllDonationsAsync();
    }
}
