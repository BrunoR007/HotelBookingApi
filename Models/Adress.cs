namespace Hoteis.Models;

public class Address : BaseClass
{
    public long AddressId { get; set; }
    
    public string Street { get; set; } = null!;
    
    public string Number { get; set; } = null!;
    
    public string? Complement { get; set; }
    
    public string Neighborhood { get; set; } = null!;
    
    public long CityId { get; set; } 
    
    public City City { get; set; } = null!;
    
    public string ZipCode { get; set; } = null!;
    
    public string Country { get; set; } = "Brasil";
    
    public double Latitude { get; set; }
    
    public double Longitude { get; set; }
}
