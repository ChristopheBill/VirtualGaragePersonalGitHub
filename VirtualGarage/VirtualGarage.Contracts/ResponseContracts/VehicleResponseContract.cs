using System;
using System.Collections.Generic;
using System.Text;

namespace VirtualGarage.Api.Contracts
{
    public sealed record VehicleResponseContract
    {
        public Guid Id { get; set; }
        public required string Brand { get; set; }
        public required string Model { get; set; }
        public DateTime Year { get; set; }
        public Guid UserId { get; set; }
    }
}
