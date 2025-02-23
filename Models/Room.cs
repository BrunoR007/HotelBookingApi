namespace Hoteis.Models;

public class Room : BaseClass
{
    public int Id { get; set; }
    
    public int HotelId { get; set; }
    
    public Hotel Hotel { get; set; } = null!;
    
    public string RoomNumber { get; set; } = null!; 
    
    public string Type { get; set; } = null!; // Ex.: "Standard", "Suite"
    
    public decimal PricePerNight { get; set; }
    
    public int Capacity { get; set; } 
    
    public string Description { get; set; } = null!; 
    
    public List<RoomPhoto> Photos { get; set; } = [];
    
    public List<Reservation> Reservations { get; set; } = []; 
}

public class RoomPhoto : BaseClass
{
    public int Id { get; set; }
    
    public int RoomId { get; set; }
    
    public Room Room { get; set; } = null!;
    
    public string Url { get; set; } = null!; 
    
    public string Description { get; set; } = null!; 
}
