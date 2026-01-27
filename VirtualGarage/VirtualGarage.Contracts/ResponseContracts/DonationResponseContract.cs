namespace VirtualGarage.Api.Contracts.ResponseContracts
{
    public class CreatePaymentIntentResponse
    {
        public string ClientSecret { get; set; } = string.Empty;
        public string PaymentIntentId { get; set; } = string.Empty;
        public int Amount { get; set; }
        public string Currency { get; set; } = string.Empty;
    }

    public class ConfirmPaymentResponse
    {
        public string Status { get; set; } = string.Empty;
        public string PaymentIntentId { get; set; } = string.Empty;
        public int Amount { get; set; }
    }
}
