using VirtualGarage.Api.Contracts.RequestContracts;
using VirtualGarage.Api.Contracts.ResponseContracts;
using VirtualGarage.Domain.Services.Interfaces;
using VirtualGarage.Persistence.Entities;
using VirtualGarage.Persistence.Interfaces;

namespace VirtualGarage.Domain.Services
{
    public class DonationService(IDonationRepository donationRepository) : IDonationService
    {
        private sealed record PaymentIntentData(string ClientSecret, int Amount, string Currency, string Email, Guid UserId);

        private static readonly Dictionary<string, PaymentIntentData> PaymentIntents = new();

        public async Task<CreatePaymentIntentResponse> CreatePaymentIntentAsync(
            CreatePaymentIntentRequest request,
            Guid userId
        )
        {
            // Validate input
            if (request.Amount <= 0)
                throw new ArgumentException("Amount must be greater than 0");

            if (string.IsNullOrWhiteSpace(request.Email))
                throw new ArgumentException("Email is required");

            // Generate a payment intent ID (in real Stripe, this comes from the API)
            string paymentIntentId = $"pi_{Guid.NewGuid().ToString("N").Substring(0, 20)}";
            string clientSecret = $"pi_{Guid.NewGuid().ToString("N").Substring(0, 20)}_secret_{Guid.NewGuid().ToString("N").Substring(0, 20)}";

            // Store the intent temporarily (in production, use actual Stripe)
            PaymentIntents[paymentIntentId] = new PaymentIntentData(clientSecret, request.Amount, request.Currency, request.Email, userId);

            return new CreatePaymentIntentResponse
            {
                PaymentIntentId = paymentIntentId,
                ClientSecret = clientSecret,
                Amount = request.Amount,
                Currency = request.Currency,
            };
        }

        public async Task<ConfirmPaymentResponse> ConfirmPaymentAsync(
            ConfirmPaymentRequest request,
            Guid userId
        )
        {
            // Validate that the payment intent exists
            if (!PaymentIntents.TryGetValue(request.PaymentIntentId, out var intentData))
                throw new InvalidOperationException("Payment intent not found");

            // Verify client secret matches
            if (intentData.ClientSecret != request.ClientSecret)
                throw new InvalidOperationException("Invalid client secret");

            // In a real scenario, you'd process with Stripe here
            // For demo: accept any valid-looking card number (starts with 4, 5, or 3)
            string cardNumber = request.Card.Number;

            // Validate card format
            if (string.IsNullOrWhiteSpace(cardNumber) || cardNumber.Length < 13)
                throw new InvalidOperationException("Invalid card number");

            // Simulate payment success for test cards
            bool isTestCard = cardNumber.StartsWith("4242") || cardNumber.StartsWith("5555");

            if (!isTestCard && !cardNumber.StartsWith("4") && !cardNumber.StartsWith("5") && !cardNumber.StartsWith("3"))
                throw new InvalidOperationException(
                    "Card declined. Use test card 4242 4242 4242 4242 for demo."
                );

            string status = "succeeded";

            // Save donation record
            var donation = new Donation
            {
                UserId = userId,
                Amount = intentData.Amount,
                Currency = intentData.Currency,
                Email = intentData.Email,
                PaymentIntentId = request.PaymentIntentId,
                Status = status,
            };

            await donationRepository.CreateDonationAsync(donation);

            return new ConfirmPaymentResponse
            {
                Status = status,
                PaymentIntentId = request.PaymentIntentId,
                Amount = intentData.Amount,
            };
        }

        public async Task<List<DonationRecordResponse>> GetAllDonationsAsync()
        {
            var donations = await donationRepository.GetAllDonationsAsync();

            return donations.Select(d => new DonationRecordResponse
            {
                Id = d.Id,
                UserId = d.UserId,
                Amount = d.Amount,
                Currency = d.Currency,
                Email = d.Email,
                Status = d.Status,
                PaymentIntentId = d.PaymentIntentId,
                CreatedAt = d.CreatedAt,
                Notes = d.Notes,
            }).ToList();
        }

        public async Task<List<DonationRecordResponse>> GetDonationsForUserAsync(Guid userId)
        {
            var donations = await donationRepository.GetDonationsByUserIdAsync(userId);

            return donations.Select(d => new DonationRecordResponse
            {
                Id = d.Id,
                UserId = d.UserId,
                Amount = d.Amount,
                Currency = d.Currency,
                Email = d.Email,
                Status = d.Status,
                PaymentIntentId = d.PaymentIntentId,
                CreatedAt = d.CreatedAt,
                Notes = d.Notes,
            }).ToList();
        }
    }
}
