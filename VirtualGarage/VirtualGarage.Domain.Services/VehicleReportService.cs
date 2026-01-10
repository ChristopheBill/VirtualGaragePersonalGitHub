using VirtualGarage.Domain.Models;
using VirtualGarage.Domain.Services.Interfaces;
using VirtualGarage.QuestPDF.Infrastructure;
using VirtualGarage.QuestPDF.Infrastructure.Interfaces;

public sealed class VehicleReportService : IVehicleReportService
{
    private readonly IVehicleSpecsProvider _specsProvider;
    private readonly IVehicleSpecsPdfGenerator _pdfGenerator;
    private readonly IBlobStorage _blobStorage;

    public VehicleReportService(
        IVehicleSpecsProvider specsProvider,
        IVehicleSpecsPdfGenerator pdfGenerator,
        IBlobStorage blobStorage)
    {
        _specsProvider = specsProvider;
        _pdfGenerator = pdfGenerator;
        _blobStorage = blobStorage;
    }

    public async Task<string> GetOrCreatePdfAsync(
    string brand,
    string model,
    int year)
{
    if (string.IsNullOrWhiteSpace(brand) || string.IsNullOrWhiteSpace(model))
        throw new ArgumentException("Brand and model must be provided.");

    var fileName = BuildFileName(brand, model, year);

    // 1️⃣ Check blob storage first
    if (await _blobStorage.ExistsAsync(fileName))
    {
        // We already know UploadAsync returns the blob URL,
        // so we can reconstruct it deterministically OR
        // add GetUriAsync later if you want to be fancy.
        return _blobStorage.GetBlobUrl(fileName);
    }

    // 2️⃣ Generate PDF
    var specs = await _specsProvider.GetSpecsAsync(brand, model, year);
    var pdfBytes = _pdfGenerator.Generate(specs);

    // 3️⃣ Store + return URL
    return await _blobStorage.UploadAsync(
        fileName,
        pdfBytes,
        contentType: "application/pdf");
    }

    public async Task<VehicleSpecs> GetRawSpecsAsync(string brand, string model, int year)
    {
        if (string.IsNullOrWhiteSpace(brand) || string.IsNullOrWhiteSpace(model))
            throw new ArgumentException("Brand and model must be provided.");

        return await _specsProvider.GetSpecsAsync(brand, model, year);
    }

    private static string BuildFileName(string brand, string model, int year)
    {   
    return $"{brand}-{model}-{year}"
        .ToLowerInvariant()
        .Replace(" ", "-")
        + ".pdf";
    }
}
