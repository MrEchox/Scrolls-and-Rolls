using Microsoft.EntityFrameworkCore;
public static class AuthEndpoints
{
    private static readonly string[] ValidRoles = { "GameMaster", "Player" };

    public static void MapAuthEndpoints(this WebApplication app)
    {
        // Register new user
        app.MapPost("/auth/register", async (MyDbContext db, UserRegistrationDto userDto) =>
        {
            // Validation
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
            if (!ValidRoles.Contains(userDto.Role))
                return Results.BadRequest("Invalid user role.");
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

            return Results.Created($"/users/{user.UserId}", user);
        })
        .WithName("RegisterUser")
        .WithDescription("Registers/Creates a new user.")
        .Produces<User>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status409Conflict)
        .WithOpenApi();

        // Login user
        app.MapPost("/auth/login", async (MyDbContext db, UserLoginDto userDto) =>
        {
            var user = await db.Users
                        .FirstOrDefaultAsync(u => u.Email == userDto.Email);

            if (user == null) return Results.NotFound("User not found.");

            if (!BCrypt.Net.BCrypt.Verify(userDto.Password, user.PasswordHash))
                return Results.Unauthorized();

            var token = new TokenService(app.Configuration).GenerateToken(user);

            return Results.Ok(token);
        })
        .WithName("LoginUser")
        .WithDescription("Logs in a user.")
        .Produces<string>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status404NotFound)
        .WithOpenApi();

        // Create admin user
        app.MapPost("/auth/createadmin", async (MyDbContext db, UserRegistrationDto userDto) =>
        {
            // Validation
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
                Role = "Admin"
            };

            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();

            return Results.Created($"/users/{user.UserId}", user);
        })
        .WithName("CreateAdminUser")
        .WithDescription("Creates a new admin user.")
        .Produces<User>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status409Conflict)
        .RequireAuthorization("Admin")
        .WithOpenApi();
    }
}
