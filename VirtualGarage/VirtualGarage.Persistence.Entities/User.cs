using System.ComponentModel.DataAnnotations;

namespace VirtualGarage.Persistence.Entities
{
    public class User
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string FirstName { get; set; }
        [MaxLength(50)]
        public string LastName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public DateTime BirthDay { get; set; }
        public enum UserRole
        {
            Admin,
            Customer,
            Mechanic
        }
        public UserRole Role { get; set; }
        public List<Vehicle>? Vehicles { get; set; }
    }
}
