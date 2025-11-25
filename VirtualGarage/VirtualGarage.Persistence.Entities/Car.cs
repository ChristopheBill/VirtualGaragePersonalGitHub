namespace VirtualGarage.Persistence.Entities;

public class Car
{
    public int Id { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public DateTime ManufactureDate { get; set; }
}