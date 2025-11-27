using System;
using System.Collections.Generic;
using System.Text;

namespace VirtualGarage.Api.Contracts
{
    public class VehicleResponseContract
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
    }
}
