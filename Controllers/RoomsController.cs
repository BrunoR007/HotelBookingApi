using Hoteis.Data;
using Hoteis.Interfaces;
using Hoteis.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hoteis.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class RoomsController(AppDbContext context, IRoomService roomService) : ControllerBase
{
    private readonly IRoomService _roomService = roomService;
    private readonly AppDbContext _context = context;

    [HttpGet("available")]
    public async Task<IActionResult> GetAvailableRooms(int hotelId, DateTime checkIn, DateTime checkOut)
    {
        var rooms = await _roomService.GetAvailableRooms(hotelId, checkIn, checkOut);
        return Ok(rooms);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRoom([FromForm] RoomDto roomDto, List<IFormFile> photos)
    {
        Room room = new()
        {
            HotelId = roomDto.HotelId,
            RoomNumber = roomDto.RoomNumber,
            Type = roomDto.Type,
            PricePerNight = roomDto.PricePerNight,
            Capacity = roomDto.Capacity,
            Description = roomDto.Description
        };

        foreach (var photo in photos)
        {
            var fileName = Guid.NewGuid() + Path.GetExtension(photo.FileName);
            var filePath = Path.Combine("wwwroot/uploads", fileName); // Ou use S3
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await photo.CopyToAsync(stream);
            }
            room.Photos.Add(new RoomPhoto { Url = $"/uploads/{fileName}", Description = "Foto do quarto" });
        }

        _context.Rooms.Add(room);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetRoom), new { id = room.Id }, room);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRoom(int id)
    {
        var room = await _context.Rooms
            .FirstOrDefaultAsync(r => r.Id == id);

        return room == null ? NotFound() : Ok(room);
    }
}

public record RoomDto(int HotelId, string RoomNumber, string Type, decimal PricePerNight, int Capacity, string Description);