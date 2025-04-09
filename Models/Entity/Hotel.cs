namespace Hoteis.Models.Entity;

public class Hotel : BaseClass
{
    public long HotelId { get; set; }

    public string HotelUserId { get; set; } = null!;

    public HotelUser HotelUser { get; set; } = null!;

    public required string Name { get; set; }

    public long AddressId { get; set; }

    public Address Address { get; set; } = null!;

    public List<Room> Rooms { get; set; } = [];
}
