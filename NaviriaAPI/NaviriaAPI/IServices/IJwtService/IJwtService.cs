using NaviriaAPI.Entities;
using System.Security.Claims;

namespace NaviriaAPI.IServices.IJwtService
{
    public interface IJwtService
    {
        string GenerateUserToken(UserEntity user);
        ClaimsPrincipal? ValidateToken(string token);
    }
}
