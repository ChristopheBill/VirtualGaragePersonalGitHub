using System;
using QuestPDF.Fluent;
using VirtualGarage.Domain.Models;
using VirtualGarage.QuestPDF.Infrastructure.Documents;
using VirtualGarage.QuestPDF.Infrastructure.Interfaces;
using VirtualGarage.QuestPDF.Infrastructure.Mapping;
using VirtualGarage.QuestPDF.Infrastructure.Storage;

namespace VirtualGarage.QuestPDF.Infrastructure;

public sealed class VehicleSpecsPdfGenerator
    : IVehicleSpecsPdfGenerator
{
    public byte[] Generate(VehicleSpecs specs)
    {
        var document = new VehicleSpecsPdfDocument(specs);

        return document.GeneratePdf();
    }
}