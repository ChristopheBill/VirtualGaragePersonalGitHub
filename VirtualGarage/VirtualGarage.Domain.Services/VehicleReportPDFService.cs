using System;
using VirtualGarage.Domain.Services.Interfaces;

namespace VirtualGarage.Domain.Services;

public sealed class VehicleReportPDFService
{
    private readonly IVehicleSpecsService _specsService;
    private readonly IPdfGenerator _pdf;

    public VehicleReportPDFService(
        IVehicleSpecsService specsService,
        IPdfGenerator pdf)
    {
        _specsService = specsService;
        _pdf = pdf;
    }

    public async Task<byte[]> CreateVehicleReportAsync(
        string brand,
        string model,
        int year)
    {
        var specs = await _specsService.GetAsync(brand, model, year);

        return _pdf.Generate(specs);
    }
}