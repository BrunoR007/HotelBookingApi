namespace Hoteis.Models;

public class Hotel : BaseClass
{
    public int Id { get; set; }
    
    public string HotelUserId { get; set; } = null!;

    public HotelUser HotelUser { get; set; } = null!;

    public required string Name { get; set; }
    
    public required string Address { get; set; }
    
    public List<Room> Rooms { get; set; } = [];
}
