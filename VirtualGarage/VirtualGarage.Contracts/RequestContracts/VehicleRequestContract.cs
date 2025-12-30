using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VirtualGarage.Api.Contracts
{
    public sealed record VehicleRequestContract
    {
        [Required, MaxLength(100)]
        public string Brand { get; set; } = null!;

        [Required, MaxLength(100)]
        public string Model { get; set; } = null!;

        [Required]
        public DateTime ManufactureDate { get; set; }
}
}
