using Hoteis.Interfaces;
using Hoteis.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Hoteis.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HotelUserController(UserManager<HotelUser> hotelUserManager, SignInManager<HotelUser> signInManager, 
    ITokenService tokenService, IConfiguration configuration) : ControllerBase
{
    private readonly UserManager<HotelUser> _hotelUserManager = hotelUserManager;
    private readonly SignInManager<HotelUser> _signInManager = signInManager;
    private readonly IConfiguration _configuration = configuration;
    private readonly ITokenService _tokenService = tokenService;

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterHotelUserDto dto)
    {
        var hotelUser = new HotelUser
        {
            UserName = dto.Email,
            Email = dto.Email,
            HotelName = dto.HotelName,
            CNPJ = dto.CNPJ,
            ContactName = dto.ContactName
        };
        var result = await _hotelUserManager.CreateAsync(hotelUser, dto.Password);

        if (result.Succeeded)
            return Ok("Hotel user registered successfully");

        return BadRequest(result.Errors);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var hotelUser = await _hotelUserManager.FindByEmailAsync(dto.Email);
        if (hotelUser == null || !await _hotelUserManager.CheckPasswordAsync(hotelUser, dto.Password))
            return Unauthorized("Invalid login");

        var token = _tokenService.GenerateJwtToken(hotelUser.Id, "HotelUser", hotelUser.Email);
        return Ok(new { Token = token });
    }
}

public record RegisterHotelUserDto(string Email, string Password, string HotelName, string CNPJ, string ContactName);