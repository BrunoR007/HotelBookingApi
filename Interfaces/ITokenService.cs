using Hoteis.Models;

namespace Hoteis.Interfaces;

public interface ITokenService
{
    string GenerateJwtToken(string userId, string userType, string? email = null);
}
