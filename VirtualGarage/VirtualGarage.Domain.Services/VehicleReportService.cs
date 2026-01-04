using VirtualGarage.Domain.Models;
using VirtualGarage.Domain.Services.Interfaces;
using VirtualGarage.QuestPDF.Infrastructure;
using VirtualGarage.QuestPDF.Infrastructure.Interfaces;

public sealed class VehicleReportService : IVehicleReportService
{
    private readonly IVehicleSpecsProvider _specsProvider;
    private readonly IVehicleSpecsPdfGenerator _pdfGenerator;

    public VehicleReportService(
        IVehicleSpecsProvider specsProvider,
        IVehicleSpecsPdfGenerator pdfGenerator)
    {
        _specsProvider = specsProvider;
        _pdfGenerator = pdfGenerator;
    }

    public async Task<byte[]> GeneratePdfAsync(
        string brand,
        string model,
        int year)
    {
        var specs = await _specsProvider
            .GetSpecsAsync(brand, model, year);

        return _pdfGenerator.Generate(specs);
    }
    public async Task<VehicleSpecs> GetRawSpecsAsync(string brand, string model, int year)
    {
        if (string.IsNullOrWhiteSpace(brand) || string.IsNullOrWhiteSpace(model))
            throw new ArgumentException("Brand and model must be provided.");

        return await _specsProvider.GetSpecsAsync(brand, model, year);
    }
}
