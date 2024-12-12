using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Cryptography;

public class TokenService
{
    private readonly string _key;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly int _expireInMinutes;
    private readonly int _rtExpireInHours;

    public TokenService(IConfiguration configuration)
    {
        _issuer = configuration["Jwt:Issuer"];
        _audience = configuration["Jwt:Audience"];
        _expireInMinutes = int.Parse(configuration["Jwt:ExpireInMinutes"]);
        _rtExpireInHours = int.Parse(configuration["RefreshToken:ExpireInHours"]);

        _key = Environment.GetEnvironmentVariable("SARJwtKey");
        if (string.IsNullOrEmpty(_key))
        {
            throw new InvalidOperationException("No JWT key found in environment variables.");
        }
    }

    public string GenerateToken(User user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(_expireInMinutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<RefreshToken> GenerateRefreshToken(User user, MyDbContext db)
    {
        var rt = new RefreshToken
        {
            RefreshTokenId = Guid.NewGuid(),
            Expires = DateTime.UtcNow.AddHours(_rtExpireInHours),
            UserId = user.UserId
        };

        db.RefreshTokens.Add(rt);
        await db.SaveChangesAsync();
        return rt;
    }

    public bool ValidateRefreshToken(MyDbContext db, User user, RefreshToken refreshToken)
    {
        var userRefreshToken = db.RefreshTokens
            .FirstOrDefault(rt => rt.RefreshTokenId == refreshToken.RefreshTokenId && rt.UserId == user.UserId && rt.Expires > DateTime.UtcNow);

        return userRefreshToken != null;
    }
}