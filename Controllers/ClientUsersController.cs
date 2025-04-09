using Hoteis.Interfaces;
using Hoteis.Models.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hoteis.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ClientUsersController(UserManager<ClientUser> userManager, SignInManager<ClientUser> signInManager,
    IEmailSender emailSender, ITokenService tokenService) : ControllerBase
{
    private readonly UserManager<ClientUser> _userManager = userManager;
    private readonly SignInManager<ClientUser> _signInManager = signInManager;
    private readonly IEmailSender _emailSender = emailSender;
    private readonly ITokenService _tokenService = tokenService;

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var user = new ClientUser { UserName = dto.Email, Email = dto.Email, FirstName = dto.FirstName };
        var result = await _userManager.CreateAsync(user, dto.Password);

        if (result.Succeeded)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, token }, Request.Scheme);
            await _emailSender.SendEmailAsync(user.Email!, "Confirm your email", $"Click <a href='{confirmationLink}'>here</a>.");
            return Ok("User registered. Check your email.");
        }

        return BadRequest(result.Errors);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var clientUser = await _userManager.FindByEmailAsync(dto.Email);
        if (clientUser == null || !await _userManager.CheckPasswordAsync(clientUser, dto.Password))
            return Unauthorized("Invalid login");

        var token = _tokenService.GenerateJwtToken(clientUser.Id, "Client", clientUser.Email);
        return Ok(new { Token = token });
    }

    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmail(string userId, string token)
    {
        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
        {
            return BadRequest("Invalid token or user ID.");
        }

        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return BadRequest("User not found.");
        }

        var result = await _userManager.ConfirmEmailAsync(user, token);

        if (result.Succeeded)
        {
            return Ok("Email confirmed successfully.");
        }

        return BadRequest("Email confirmation failed.");
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
        {
            return BadRequest("User not found.");
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var resetLink = Url.Action("ResetPassword", "Account", new { userId = user.Id, token }, protocol: HttpContext.Request.Scheme);

        await _emailSender.SendEmailAsync(user.Email!, "Reset your password", $"Please reset your password by clicking <a href='{resetLink}'>here</a>.");

        return Ok("Password reset link sent to your email.");
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(string userId, string token, string newPassword)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return BadRequest("User not found.");
        }

        var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

        if (result.Succeeded)
        {
            return Ok("Password reset successfully.");
        }

        return BadRequest("Password reset failed.");
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();

        return Ok("Logged out successfully.");
    }
}

public record RegisterDto(string Email, string Password, string FirstName);
public record LoginDto(string Email, string Password);