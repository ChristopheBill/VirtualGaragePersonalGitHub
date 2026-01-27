using VirtualGarage.Api.Contracts.RequestContracts;
using VirtualGarage.Api.Contracts.ResponseContracts;

namespace VirtualGarage.Domain.Services.Interfaces
{
    public interface IDonationService
    {
        Task<CreatePaymentIntentResponse> CreatePaymentIntentAsync(
            CreatePaymentIntentRequest request,
            Guid userId
        );

        Task<ConfirmPaymentResponse> ConfirmPaymentAsync(
            ConfirmPaymentRequest request,
            Guid userId
        );

        Task<List<DonationRecordResponse>> GetAllDonationsAsync();

        Task<List<DonationRecordResponse>> GetDonationsForUserAsync(Guid userId);
    }
}
