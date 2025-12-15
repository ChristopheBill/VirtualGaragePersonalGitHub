using System;
using System.Collections.Generic;
using System.Text;

namespace VirtualGarage.Api.Contracts
{
    public class UserResponseContract
    {
        public Guid Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public List<VehicleResponseContract>? Vehicles { get; set; }
    }
}
