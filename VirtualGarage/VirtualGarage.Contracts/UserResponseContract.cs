using System;
using System.Collections.Generic;
using System.Text;

namespace VirtualGarage.Api.Contracts
{
    public class UserResponseContract
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public List<VehicleResponseContract> Vehicles { get; set; }
    }
}
