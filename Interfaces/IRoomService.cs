using Hoteis.Models.Entity;

namespace Hoteis.Interfaces;

public interface IRoomService
{
    Task<List<Room>> GetAvailableRooms(int hotelId, DateTime checkIn, DateTime checkOut);
}
