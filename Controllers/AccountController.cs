using Habitus.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Habitus.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController(UserManager<User> userManager, SignInManager<User> signInManager) : ControllerBase
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly SignInManager<User> _signInManager = signInManager;

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] Account model)
    {
        var user = new User { UserName = model.UserName, Email = model.Email };
        var result = await _userManager.CreateAsync(user, model.PasswordHash!);

        if (result.Succeeded)
            return Ok("User registered successfully");

        return BadRequest(result.Errors);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] Account model)
    {
        var result = await _signInManager.PasswordSignInAsync(model.Email!, model.PasswordHash!, false, false);

        if (result.Succeeded)
            return Ok("Login successful");

        return Unauthorized("Invalid login attempt");
    }
}
