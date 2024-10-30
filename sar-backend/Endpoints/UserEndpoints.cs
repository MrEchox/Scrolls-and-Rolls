using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        // Get specific user
        app.MapGet("/users/{userId}", async (MyDbContext db, Guid userId) =>
        {
            var session = await db.Users
                                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (session == null) return Results.NotFound();

            return Results.Ok(session);
        })
        .WithName("GetUserById")
        .WithDescription("Gets game session by ID.")
        .Produces<Session>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .RequireAuthorization("LoggedIn")
        .WithOpenApi();

        // Update user
        app.MapPut("/users/{userId}", async (MyDbContext db, Guid userId, User user) =>
        {
            var existingUser = await db.Users
                                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (existingUser == null) return Results.NotFound();

            existingUser.Username = user.Username;
            existingUser.PasswordHash = user.PasswordHash;
            existingUser.Email = user.Email;
            existingUser.Role = user.Role;

            await db.SaveChangesAsync();

            return Results.Ok(existingUser);
        })
        .WithName("UpdateUser")
        .WithDescription("Updates a user.")
        .Accepts<User>("The user to update.")
        .Produces<User>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .RequireAuthorization("LoggedIn")
        .WithOpenApi();

        // Delete user
        app.MapDelete("/users/{userId}", async (MyDbContext db, Guid userId) =>
        {
            var existingUser = await db.Users
                                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (existingUser == null) return Results.NotFound();

            db.Users.Remove(existingUser);
            await db.SaveChangesAsync();

            return Results.NoContent();
        })
        .WithName("DeleteUser")
        .WithDescription("Deletes a user.")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
        .RequireAuthorization("LoggedIn")
        .WithOpenApi();
    }
}
