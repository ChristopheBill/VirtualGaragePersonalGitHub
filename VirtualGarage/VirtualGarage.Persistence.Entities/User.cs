using System.ComponentModel.DataAnnotations;

namespace VirtualGarage.Persistence.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public DateTime BirthDay { get; set; }
        
    }
}
