using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Hoteis.Models.Entity;

public class ClientUser : IdentityUser
{
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public DateTime? DateOfBirth { get; set; }

    public string? Gender { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? ZipCode { get; set; } // CEP, ex.: "12345-678"

    public List<Reservation> Reservations { get; set; } = [];

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;

    [RegularExpression(@"^\d{11}$", ErrorMessage = "CPF deve ter 11 dígitos.")]
    public string? CPF { get; set; }
}