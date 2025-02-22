using Hoteis.Data;
using Hoteis.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hoteis.Controllers;

[ApiController]
[Route("api/usuarios")]
public class UsersController(AppDbContext context) : ControllerBase
{
    private readonly AppDbContext _context = context;

    [HttpGet]
    public IActionResult ObterUsuarios()
    {
        return Ok(_context.Users.ToList());
    }

    [HttpPost]
    public IActionResult CriarUsuario([FromBody] User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
        return CreatedAtAction(nameof(ObterUsuarios), new { id = user.Id }, user);
    }
}