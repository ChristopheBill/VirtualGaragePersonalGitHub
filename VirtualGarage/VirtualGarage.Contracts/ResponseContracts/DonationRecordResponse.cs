namespace VirtualGarage.Api.Contracts.ResponseContracts
{
    public class DonationRecordResponse
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public int Amount { get; set; }
        public string Currency { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string PaymentIntentId { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string? Notes { get; set; }
    }
}
