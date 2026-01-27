using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using VirtualGarage.Shared;


namespace VirtualGarage.Persistence.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        [MaxLength(50)]
        public required string FirstName { get; set; }
        [MaxLength(50)]
        public required string LastName { get; set; }
        [EmailAddress]
        public required string Email { get; set; }
        public DateTime BirthDay { get; set; }
        public RoleEnum UserRole { get; set; }
        [JsonIgnore]
        public List<Vehicle>? Vehicles { get; set; }
    }
}
