﻿using Microsoft.EntityFrameworkCore;
public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        // Register new user
        app.MapPost("/auth/register", async (MyDbContext db, UserRegistrationDto userDto) =>
        {
            var existingUser = await db.Users
                                .FirstOrDefaultAsync(u => u.Email == userDto.Email);

            if (existingUser != null) return Results.Conflict("Email already in use.");

            var user = new User
            {
                Username = userDto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
                Email = userDto.Email,
                Role = UserRole.Player // Default role
            };

            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();

            return Results.Created($"/users/{user.UserId}", user);
        })
        .WithName("RegisterUser")
        .WithDescription("Registers/Creates a new user.")
        .Accepts<UserRegistrationDto>("The user to register/create.")
        .Produces<User>(StatusCodes.Status201Created)
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
        .Accepts<UserLoginDto>("The user to login.")
        .Produces<string>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status404NotFound)
        .WithOpenApi();
    }
}