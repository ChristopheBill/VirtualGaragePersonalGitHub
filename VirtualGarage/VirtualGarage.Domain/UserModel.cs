using System;

namespace VirtualGarage.Domain.Models;

public class UserModel
{
    public Guid Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public DateTime BirthDay { get; set; }
    public required RoleEnum Role { get; set; }
}
