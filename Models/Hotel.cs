namespace Hoteis.Models;

public class Hotel
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Address { get; set; }
    public List<Room> Rooms { get; set; } = [];
}
