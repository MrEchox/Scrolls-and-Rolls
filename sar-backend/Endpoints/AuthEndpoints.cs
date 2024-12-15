using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
public static class AuthEndpoints
{
    private static readonly string[] ValidRoles = { "GameMaster", "Player" };

    public static void MapAuthEndpoints(this WebApplication app)
    {
        // Register new user
        app.MapPost("/api/auth/register", async (MyDbContext db, UserRegistrationDto userDto) =>
        {
            // Validation
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(userDto);

            if (!Validator.TryValidateObject(userDto, validationContext, validationResults, true))
            {
                return Results.BadRequest(validationResults);
            }

            var existingUser = await db.Users
            .Where(u => u.Email == userDto.Email || u.Username == userDto.Username)
            .Select(u => new { u.Email, u.Username })
            .FirstOrDefaultAsync();

            if (existingUser != null)
            {
                if (existingUser.Email == userDto.Email)
                    return Results.Conflict("Email already in use.");

                if (existingUser.Username == userDto.Username)
                    return Results.Conflict("Username already in use.");
            }
            // ----------------

            var user = new User
            {
                Username = userDto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
                Email = userDto.Email,
                Role = userDto.Role
            };

            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();

            return Results.Created($"/api/users/{user.UserId}", user);
        })
        .WithName("RegisterUser")
        .WithDescription("Registers/Creates a new user.")
        .Produces<User>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status409Conflict)
        .WithOpenApi();

        // Login user
        app.MapPost("/api/auth/login", async (MyDbContext db, UserLoginDto userDto) =>
        {
            var user = await db.Users
                        .FirstOrDefaultAsync(u => u.Email == userDto.Email);

            if (user == null) return Results.NotFound("User not found.");

            if (!BCrypt.Net.BCrypt.Verify(userDto.Password, user.PasswordHash))
                return Results.Unauthorized();

            var token = new TokenService(app.Configuration).GenerateToken(user);
            var refreshToken = await new TokenService(app.Configuration).GenerateRefreshToken(user, db);

            return Results.Ok(new {token, refreshToken = refreshToken.RefreshTokenId});
        })
        .WithName("LoginUser")
        .WithDescription("Logs in a user.")
        .Produces<string>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status404NotFound)
        .WithOpenApi();

        // Refresh token
        app.MapPost("/api/auth/refresh", async (MyDbContext db, RefreshTokenDto refreshTokenDto) =>
        {
            var refreshToken = await db.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.RefreshTokenId == refreshTokenDto.RefreshTokenId);
            if (refreshToken == null) return Results.NotFound("Refresh token not found.");

            var user = await db.Users
                .FirstOrDefaultAsync(u => u.UserId == refreshToken.UserId);
            if (user == null) return Results.NotFound("User not found.");

            if (!new TokenService(app.Configuration).ValidateRefreshToken(db, user, refreshToken))
                return Results.Unauthorized();

            var token = new TokenService(app.Configuration).GenerateToken(user);

            return Results.Ok(new { token, refreshToken = refreshTokenDto.RefreshTokenId });
        })
        .WithName("RefreshToken")
        .WithDescription("Refreshes a token.")
        .Produces<string>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status404NotFound)
        .WithOpenApi();

        // Create admin user
        app.MapPost("/api/auth/createadmin", async (MyDbContext db, AdminRegistrationDto adminDto) =>
        {
            // Validation
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(adminDto);

            if (!Validator.TryValidateObject(adminDto, validationContext, validationResults, true))
            {
                return Results.BadRequest(validationResults);
            }

            var existingUser = await db.Users
            .Where(u => u.Email == adminDto.Email || u.Username == adminDto.Username)
            .Select(u => new { u.Email, u.Username })
            .FirstOrDefaultAsync();

            if (existingUser != null)
            {
                if (existingUser.Email == adminDto.Email)
                    return Results.Conflict("Email already in use.");

                if (existingUser.Username == adminDto.Username)
                    return Results.Conflict("Username already in use.");
            }
            // ----------------

            var user = new User
            {
                Username = adminDto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(adminDto.Password),
                Email = adminDto.Email,
                Role = "Admin"
            };

            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();

            return Results.Created($"/api/users/{user.UserId}", user);
        })
        .WithName("CreateAdminUser")
        .WithDescription("Creates a new admin user.")
        .Produces<User>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status409Conflict)
        .RequireAuthorization("Admin")
        .WithOpenApi();
    }
}
