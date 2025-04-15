namespace Hoteis.Models.Entity;

public class Room : BaseClass
{
    public long RoomId { get; set; }

    public long HotelId { get; set; }

    public Hotel Hotel { get; set; } = null!;

    public string RoomNumber { get; set; } = null!;

    public decimal PricePerNight { get; set; }

    public int Capacity { get; set; }
    
    public string Description { get; set; } = null!;

    public int RoomTypeId { get; set; }

    public List<RoomPhoto> Photos { get; set; } = [];

    public List<Reservation> Reservations { get; set; } = [];


    public RoomType RoomType { get; set; } = null!;
}

public class RoomPhoto : BaseClass
{
    public long RoomPhotoId { get; set; }

    public long RoomId { get; set; }

    public Room Room { get; set; } = null!;

    public string Url { get; set; } = null!;

    public string Description { get; set; } = null!;
}
