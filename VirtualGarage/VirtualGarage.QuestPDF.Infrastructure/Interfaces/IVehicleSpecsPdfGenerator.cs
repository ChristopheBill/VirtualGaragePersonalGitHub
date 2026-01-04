using System;
using VirtualGarage.Domain.Models;

namespace VirtualGarage.QuestPDF.Infrastructure.Interfaces;

public interface IVehicleSpecsPdfGenerator
{
    byte[] Generate(VehicleSpecs specs);
}
