using System;
using QuestPDF.Fluent;
using VirtualGarage.Domain.Models;
using VirtualGarage.QuestPDF.Infrastructure.Documents;

namespace VirtualGarage.QuestPDF.Infrastructure;

public sealed class VehicleSpecsPdfGenerator
    : IVehicleSpecsPdfGenerator
{
    public byte[] Generate(VehicleSpecs specs)
    {
        var document = new VehicleSpecsDocument(specs);

        return document.GeneratePdf();
    }
}