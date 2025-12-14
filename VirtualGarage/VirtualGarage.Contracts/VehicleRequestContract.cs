using System;
using System.Collections.Generic;
using System.Text;

namespace VirtualGarage.Api.Contracts
{
    public class VehicleRequestContract
    {
        public required string Brand { get; set; }
        public required string Model { get; set; }
        public int Year { get; set; }
    }
}
