using System.ComponentModel.DataAnnotations;

namespace VirtualGarage.Persistence.Entities
{
    public class Donation
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public int Amount { get; set; } // in cents

        [Required]
        [MaxLength(10)]
        public string Currency { get; set; } = "usd";

        [Required]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string PaymentIntentId { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "completed"; // pending, completed, failed

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string? Notes { get; set; }
    }
}
