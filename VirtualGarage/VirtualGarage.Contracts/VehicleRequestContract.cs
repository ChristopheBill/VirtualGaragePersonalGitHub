using System;
using System.Collections.Generic;
using System.Text;

namespace VirtualGarage.Api.Contracts
{
    public class VehicleRequestContract
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
    }
}
