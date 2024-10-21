using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

// TODO: Read below
/// <summary>
/// Needs validation logic to check user credentials against the database.
/// Needs protection with [Authorize] attribute on endpoints that require authentication.
/// Implement user registration and password hashing.
/// </summary>


[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;

    public AuthController(IConfiguration config)
    {
        _config = config;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel login)
    {
        // Validate the user credentials (retrieve from DB)
        // This is just a placeholder, replace with actual user validation logic
        var user = ValidateUser(login.Username, login.Password);

        if (user == null)
            return Unauthorized();

        // Generate JWT token
        var token = GenerateJwtToken(user);
        return Ok(new { Token = token });
    }

    private string GenerateJwtToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role) // Save role in the token
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SARJwtKey"))); 
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(_config["Jwt:ExpiresInMinutes"])),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private User ValidateUser(string username, string password)
    {
        // Check credentials against the database and return user
        // For demonstration, returning a dummy user
        return new User
        {
            UserId = Guid.NewGuid(),
            Username = username,
            Role = "Player" // Replace with actual role from DB
        };
    }
}

public class LoginModel
{
    public string Username { get; set; }
    public string Password { get; set; }
}

