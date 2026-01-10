using System;
using VirtualGarage.Domain.Models;

namespace VirtualGarage.QuestPDF.Infrastructure.Interfaces;
public interface IVehicleSpecsProvider
{
    Task<VehicleSpecs> GetSpecsAsync(
        string brand,
        string model,
        int year);
}