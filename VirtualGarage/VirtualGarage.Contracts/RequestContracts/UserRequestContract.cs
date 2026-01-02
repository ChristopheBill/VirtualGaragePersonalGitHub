using System.ComponentModel.DataAnnotations;

namespace VirtualGarage.Contracts
{
    public sealed record UserRequestContract
    {
        [Required, MaxLength(50)]
        public required string FirstName { get; set; }
        [Required, MaxLength(50)]
        public required string LastName { get; set; }
        [Required, EmailAddress]
        public required string Email { get; set; }

    }
}
