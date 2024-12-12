using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Security.Claims;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        // Get specific user
        app.MapGet("/users/{userId}", async (MyDbContext db, Guid userId, HttpContext httpContext) =>
        {
            var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            var userRoleClaim = httpContext.User.FindFirst(ClaimTypes.Role);

            if (userIdClaim == null || userRoleClaim == null)
                return Results.Unauthorized();

            string role = userRoleClaim.Value;
            string loggedInUserId = userIdClaim.Value;

            if (string.IsNullOrEmpty(role) || string.IsNullOrEmpty(loggedInUserId))
                return Results.Unauthorized();

            if (role != "Admin" && userId.ToString() != loggedInUserId) return Results.Unauthorized();

            var user = await db.Users.FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null) return Results.NotFound();

            return Results.Ok(user);
        })
        .WithName("GetUserById")
        .WithDescription("Gets user by ID.")
        .Produces<User>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status401Unauthorized)
        .RequireAuthorization("LoggedIn")
        .WithOpenApi();

        // Update user
        app.MapPut("/users/{userId}", async (MyDbContext db, Guid userId, User user, HttpContext httpContext) =>
        {
            var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            var userRoleClaim = httpContext.User.FindFirst(ClaimTypes.Role);

            if (userIdClaim == null || userRoleClaim == null)
                return Results.Unauthorized();

            string role = userRoleClaim.Value;
            string loggedInUserId = userIdClaim.Value;

            if (string.IsNullOrEmpty(role) || string.IsNullOrEmpty(loggedInUserId))
                return Results.Unauthorized();

            if (role != "Admin" && userId.ToString() != loggedInUserId) return Results.Unauthorized();

            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(user);

            if (!Validator.TryValidateObject(user, validationContext, validationResults, true))
            {
                return Results.BadRequest(validationResults);
            }

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
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status404NotFound)
        .RequireAuthorization("LoggedIn")
        .WithOpenApi();

        // Delete user
        app.MapDelete("/users/{userId}", async (MyDbContext db, Guid userId, HttpContext httpContext) =>
        {
            var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            var userRoleClaim = httpContext.User.FindFirst(ClaimTypes.Role);

            if (userIdClaim == null || userRoleClaim == null)
                return Results.Unauthorized();

            string role = userRoleClaim.Value;
            string loggedInUserId = userIdClaim.Value;

            if (string.IsNullOrEmpty(role) || string.IsNullOrEmpty(loggedInUserId))
                return Results.Unauthorized();

            if (role != "Admin" && userId.ToString() != loggedInUserId) return Results.Unauthorized();

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
