using Hoteis.Models;

namespace Hoteis.Interfaces;

public interface IRoomService
{
    Task<List<Room>> GetAvailableRooms(int hotelId, DateTime checkIn, DateTime checkOut);
}
