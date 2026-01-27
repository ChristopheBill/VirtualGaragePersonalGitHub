namespace VirtualGarage.Api.Contracts.RequestContracts
{
    public class CreatePaymentIntentRequest
    {
        public int Amount { get; set; } // in cents
        public string Email { get; set; } = string.Empty;
        public string Currency { get; set; } = "usd";
    }

    public class ConfirmPaymentRequest
    {
        public string PaymentIntentId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
        public CardDetails Card { get; set; } = new();
    }

    public class CardDetails
    {
        public string Number { get; set; } = string.Empty;
        public string Exp_Month { get; set; } = string.Empty;
        public string Exp_Year { get; set; } = string.Empty;
        public string Cvc { get; set; } = string.Empty;
    }
}
