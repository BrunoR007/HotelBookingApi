using Hoteis.Data;
using Hoteis.Models.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Hoteis.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ReservationsController(AppDbContext context) : ControllerBase
{
    private readonly AppDbContext _context = context;

    [HttpPost]
    public async Task<IActionResult> CreateReservation([FromBody] ReservationDto dto)
    {
        var room = await _context.Rooms
            .Include(r => r.Reservations)
            .FirstOrDefaultAsync(r => r.RoomId == dto.RoomId);

        if (room == null) return NotFound("Quarto não encontrado");

        if (room.Reservations.Any(r => dto.CheckIn < r.CheckOut && dto.CheckOut > r.CheckIn))
            return BadRequest("Quarto não disponível nas datas solicitadas");

        Reservation reservation = new() 
        {
            ClientUserId = User.FindFirstValue(JwtRegisteredClaimNames.Sub)!,
            RoomId = dto.RoomId,
            CheckIn = dto.CheckIn,
            CheckOut = dto.CheckOut,
            TotalPrice = (dto.CheckOut - dto.CheckIn).Days * room.PricePerNight
        };

        _context.Reservations.Add(reservation);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetReservation), new { id = reservation.ReservationId }, reservation);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetReservation(int id)
    {
        var reservation = await _context.Reservations
            .Include(r => r.Room)
            .FirstOrDefaultAsync(r => r.ReservationId == id);

        return reservation == null ? NotFound() : Ok(reservation);
    }
}

public record ReservationDto(long RoomId, DateTime CheckIn, DateTime CheckOut);