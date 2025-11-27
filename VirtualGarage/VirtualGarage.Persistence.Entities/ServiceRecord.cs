using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualGarage.Persistence.Entities
{
    public class ServiceRecord
    {
        public int Id { get; set; }
        public DateTime ServiceDate { get; set; }
        public string Description { get; set; }
        public decimal Cost { get; set; }
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }
    }
}
