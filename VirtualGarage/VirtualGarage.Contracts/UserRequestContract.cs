using System.ComponentModel.DataAnnotations;

namespace VirtualGarage.Contracts
{
    public class UserRequestContract
    {
        [MaxLength(50)]
        public required string FirstName { get; set; }
        [MaxLength(50)]
        public required string LastName { get; set; }
        [EmailAddress]
        public required string Email { get; set; }

    }
}
