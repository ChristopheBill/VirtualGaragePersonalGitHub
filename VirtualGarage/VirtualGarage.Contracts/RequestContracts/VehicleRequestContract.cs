using System;
using System.Collections.Generic;
using System.Text;

namespace VirtualGarage.Api.Contracts
{
    public class VehicleRequestContract
    {
        public required string Make { get; set; }
        public required string Model { get; set; }
        public required DateTime ManufactureDate { get; set; }
        public int Year { get; set; }
    }
}
