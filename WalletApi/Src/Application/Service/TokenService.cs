using System.IdentityModel.Tokens.Jwt;

namespace WalletApi.Application.Service;

public class TokenService(IHttpContextAccessor httpContextAccessor)
{
    public string GetUserEmailFromToken()
    {
        var token = httpContextAccessor.HttpContext?.Request.Headers.Authorization.ToString().Replace("Bearer ", "");

        if (string.IsNullOrEmpty(token))
        {
            throw new UnauthorizedAccessException("Authentication token not found");
        }

        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

        var emailClaim = jsonToken?.Claims.FirstOrDefault(c => c.Type == "email")?.Value;

        if (emailClaim == null)
        {
            throw new UnauthorizedAccessException("Email claim not found");
        }

        return emailClaim;
    }

    public string GetToken()
    {
        return httpContextAccessor.HttpContext?.Request.Headers.Authorization!;
    }
}