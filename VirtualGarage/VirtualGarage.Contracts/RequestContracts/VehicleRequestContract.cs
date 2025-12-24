using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VirtualGarage.Api.Contracts
{
    public class VehicleRequestContract
    {
        [Required]
        public required Guid OwnerId { get; set; }
        [Required, MaxLength(50)]
        public required string Make { get; set; }
        [Required, MaxLength(50)]
        public required string Model { get; set; }
        [Required]
        public required DateTime Year { get; set; }
    }
}
