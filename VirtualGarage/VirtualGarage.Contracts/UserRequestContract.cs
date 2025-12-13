using System.ComponentModel.DataAnnotations;

namespace VirtualGarage.Contracts
{
    public class UserRequestContract
    {
        [MaxLength(50)]
        public string FirstName { get; set; }
        [MaxLength(50)]
        public string LastName { get; set; }
        [EmailAddress]
        public string Email { get; set; }

    }
}
