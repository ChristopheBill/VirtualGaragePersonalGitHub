using System.ComponentModel.DataAnnotations;

namespace VirtualGarage.Persistence.Entities;

public class Vehicle
{
    public int Id { get; set; }
    [MaxLength(50)]
    public string? Brand { get; set; }
    [MaxLength(50)]
    public string? Model { get; set; }
    public DateTime ManufactureDate { get; set; }
    public List<ServiceRecord>? ServiceRecords { get; set; }
}