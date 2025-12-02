using System.ComponentModel.DataAnnotations;

namespace VirtualGarage.Persistence.Entities;

public class Vehicle
{
    public Guid Id { get; set; }
    [MaxLength(50)]
    public Guid? UserId { get; set; }
    public string? Brand { get; set; }
    [MaxLength(50)]
    public string? Model { get; set; }
    public DateTime ManufactureDate { get; set; }
    public List<ServiceRecord>? ServiceRecords { get; set; }
}