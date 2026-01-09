using System;
using VirtualGarage.Domain.Models;

namespace VirtualGarage.Domain.Services.Interfaces;

public interface IVehicleReportService
{
    Task<string> GetOrCreatePdfAsync(string brand, string model, int year);
    Task<VehicleSpecs> GetRawSpecsAsync(string brand, string model, int year);
}
