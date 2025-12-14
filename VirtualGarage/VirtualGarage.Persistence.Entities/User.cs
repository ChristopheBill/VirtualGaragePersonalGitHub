using System.ComponentModel.DataAnnotations;
using VirtualGarage.Shared;


namespace VirtualGarage.Persistence.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        [MaxLength(50)]
        public string? FirstName { get; set; }
        [MaxLength(50)]
        public string? LastName { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        public DateTime BirthDay { get; set; }
        public RoleEnum UserRole { get; set; }
        public List<Vehicle>? Vehicles { get; set; }
    }
}
