using System;
using System.Collections.Generic;
using System.Text;

namespace VirtualGarage.Api.Contracts
{
    public class VehicleResponseContract
    {
        public Guid Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public DateTime Year { get; set; }
        public Guid UserId { get; set; }
    }
}
