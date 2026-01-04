using System;
using VirtualGarage.Domain.Models;
using VirtualGarage.QuestPDF.Infrastructure.Documents;
using VirtualGarage.QuestPDF.Infrastructure.Storage;

namespace VirtualGarage.QuestPDF.Infrastructure.Mapping;

internal static class VehicleSpecsMapper
{
    public static VehicleSpecsDocument ToDocument(
        VehicleSpecs specs)
    {
        return new VehicleSpecsDocument
        {
            Id = $"{specs.Brand}-{specs.Model}-{specs.Year}".ToLowerInvariant(),

            Brand = specs.Brand,
            Model = specs.Model,
            Year = specs.Year,

            Engine = specs.Engine,
            HorsePower = specs.HorsePower,
            FuelType = specs.FuelType,

            Transmission = specs.Transmission,
            Doors = specs.Doors,
            Seats = specs.Seats,
            DriveType = specs.DriveType,

            RetrievedAt = DateTime.UtcNow,
        };
    }

    public static VehicleSpecs ToDomain(VehicleSpecsDocument doc)
    {
        return new VehicleSpecs
        {
            Brand = doc.Brand,
            Model = doc.Model,
            Year = doc.Year,

            Engine = doc.Engine,
            HorsePower = doc.HorsePower,
            FuelType = doc.FuelType,

            Transmission = doc.Transmission,
            Doors = doc.Doors,
            Seats = doc.Seats,
            DriveType = doc.DriveType
        };
    }
}