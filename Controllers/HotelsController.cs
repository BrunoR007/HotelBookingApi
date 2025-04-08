using Hoteis.Data;
using Hoteis.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hoteis.Controllers;

//[Authorize]
[ApiController]
[Route("api/[controller]")]
public class HotelsController(AppDbContext context) : ControllerBase
{
    private readonly AppDbContext _context = context;

    [HttpPost]
    public async Task<IActionResult> CreateHotel([FromBody] Hotel hotel)
    {
        _context.Hotels.Add(hotel);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetHotel), new { id = hotel.HotelId }, hotel);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetHotel(long id)
    {
        var hotel = await _context.Hotels.FindAsync(id);
        return hotel == null ? NotFound() : Ok(hotel);
    }
}
