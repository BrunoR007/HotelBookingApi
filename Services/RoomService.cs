using Hoteis.Data;
using Hoteis.Interfaces;
using Hoteis.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace Hoteis.Services;

public class RoomService(AppDbContext context) : IRoomService
{
    private readonly AppDbContext _context = context;

    public async Task<List<Room>> GetAvailableRooms(int hotelId, DateTime checkIn, DateTime checkOut)
    {
        var rooms = await _context.Rooms
            .Where(r => r.HotelId == hotelId)
            .Include(r => r.Reservations)
            .ToListAsync();

        var availableRooms = rooms.Where(room => !room.Reservations.Any(res =>
            checkIn < res.CheckOut && checkOut > res.CheckIn
        )).ToList();

        return availableRooms;
    }
}
