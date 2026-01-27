using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace VirtualGarage.Persistence.Entities;

public class Vehicle
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    [JsonIgnore]
    public User? User { get; set; } // navigation property
    [MaxLength(50)]
    public required string Brand { get; set; }
    [MaxLength(50)]
    public required string Model { get; set; }
    public DateTime ManufactureDate { get; set; }
    // public List<ServiceRecord>? ServiceRecords { get; set; }
}