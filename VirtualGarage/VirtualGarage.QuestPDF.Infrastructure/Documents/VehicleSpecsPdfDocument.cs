using System;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using VirtualGarage.Domain.Models;
using VirtualGarage.QuestPDF.Infrastructure.Mapping;

namespace VirtualGarage.QuestPDF.Infrastructure.Documents;

public sealed class VehicleSpecsPdfDocument : IDocument
{
    private readonly VehicleSpecs _specs;

    public VehicleSpecsPdfDocument(VehicleSpecs specs)
    {
        _specs = specs;
    }

    public DocumentMetadata GetMetadata()
        => DocumentMetadata.Default;

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Margin(40);

            page.Header()
                .Text($"{_specs.Brand} {_specs.Model} ({_specs.Year})")
                .FontSize(20)
                .SemiBold();

            page.Content().Column(column =>
            {
                column.Spacing(10);

                SpecRow(column, "Engine", _specs.Engine);
                SpecRow(column, "Horsepower", $"{_specs.HorsePower} HP");
                SpecRow(column, "Fuel", _specs.FuelType);
                SpecRow(column, "Transmission", _specs.Transmission);
                SpecRow(column, "Drive Type", _specs.DriveType);
                SpecRow(column, "Doors", _specs.Doors?.ToString());
                SpecRow(column, "Seats", _specs.Seats?.ToString());
            });
        });
    }
    private static void SpecRow(ColumnDescriptor column, string label, string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return;

        column.Item().Row(row =>
        {
            row.RelativeItem(1)
                .Text(label)
                .SemiBold();

            row.RelativeItem(3)
                .Text(value);
        });
    }
}