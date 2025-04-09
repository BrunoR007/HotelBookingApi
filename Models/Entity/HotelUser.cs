using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Hoteis.Models.Entity;

public class HotelUser : IdentityUser
{
    [Required]
    public string HotelName { get; set; } = null!;

    [Required]
    public string CNPJ { get; set; } = null!;

    public string ContactName { get; set; } = null!;

    public List<Hotel> ManagedHotels { get; set; } = [];

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime ModifiedAt { get; set; }
}
