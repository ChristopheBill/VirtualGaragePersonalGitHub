using VirtualGarage.Domain.Services.Interfaces;
using VirtualGarage.Shared.Interfaces;

public sealed class VehicleReportService
{
    private readonly VirtualGarage.Domain.Services.Interfaces.IVehicleSpecsProvider _specsProvider;
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
}
