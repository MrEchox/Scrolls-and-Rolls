using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Security.Claims;

public static class SessionEndpoints
{
    public static void MapSessionEndpoints(this WebApplication app)
    {
        // Get all sessions or a GameMaster's own sessions
        app.MapGet("/api/sessions", async (MyDbContext db, HttpContext httpContext) =>
        {
            var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            var userRoleClaim = httpContext.User.FindFirst(ClaimTypes.Role);

            if (userIdClaim == null || userRoleClaim == null)
                return Results.Text("No claims");

            string role = userRoleClaim.Value;
            string userId = userIdClaim.Value;

            if (string.IsNullOrEmpty(role) || string.IsNullOrEmpty(userId))
                return Results.Text("No strings");


            IQueryable<Session> query = db.Sessions.Include(s => s.Items);

            if (role == "Admin")
            {
                // Admin can get all sessions
                query = query;
            }
            else if (role == "GameMaster")
            {
                // GameMaster can only get their own sessions
                query = query.Where(s => s.GameMasterId.ToString() == userId);
            }
            else
            {
                return Results.Unauthorized(); // Unauthorized for other roles
            }

            var sessions = await query.ToListAsync();
            return Results.Ok(sessions);
        })
        .WithName("GetSessions")
        .WithDescription("Gets game sessions based on user role (Admin: all, GameMaster: own).")
        .Produces<Session>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .RequireAuthorization("LoggedIn") // Use this to ensure only logged-in users can access
        .WithOpenApi();


        // Get specific session
        app.MapGet("/api/sessions/{sessionId}", async (MyDbContext db, Guid sessionId) =>
        {
            var session = await db.Sessions
                                .Include(s => s.Items)
                                .FirstOrDefaultAsync(s => s.SessionId == sessionId);

            if (session == null) return Results.NotFound();

            return Results.Ok(session);
        })
        .WithName("GetSessionById")
        .WithDescription("Gets game session by ID.")
        .Produces<Session>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .RequireAuthorization("LoggedIn")
        .WithOpenApi();

        // Create session
        app.MapPost("/api/sessions", async (MyDbContext db, Session s) =>
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(s);

            // Validate the object
            if (!Validator.TryValidateObject(s, validationContext, validationResults, true))
            {
                // Return bad request if validation fails
                return Results.BadRequest(validationResults);
            }

            // If validation passes, create the session
            var session = new Session
            {
                SessionId = Guid.NewGuid(),
                SessionName = s.SessionName,
                GameMasterId = s.GameMasterId
            };

            db.Sessions.Add(session);
            await db.SaveChangesAsync();

            // Return the created session with a 201 status
            return Results.Created($"/api/sessions/{session.SessionId}", session);
        })
        .WithName("CreateSession")
        .WithDescription("Creates a new game session.")
        .Produces<Session>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .RequireAuthorization("GameMaster")
        .WithOpenApi();


        // Update session
        app.MapPut("/api/sessions/{sessionId}", async (MyDbContext db, Guid sessionId, Session s) =>
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(s);

            if (!Validator.TryValidateObject(s, validationContext, validationResults, true))
            {
                return Results.BadRequest(validationResults);
            }

            var session = await db.Sessions.FindAsync(sessionId);
            if (session == null) return Results.NotFound();

            session.SessionName = s.SessionName;
            session.GameMasterId = s.GameMasterId;
            await db.SaveChangesAsync();

            return Results.Ok(session);
        })
        .WithName("UpdateSession")
        .WithDescription("Updates a game session by ID.")
        .Produces<Session>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound)
        .RequireAuthorization("Admin")
        .WithOpenApi();

        // Delete session
        app.MapDelete("/api/sessions/{sessionId}", async (MyDbContext db, Guid sessionId, HttpContext httpContext) =>
        {
            var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            var userRoleClaim = httpContext.User.FindFirst(ClaimTypes.Role);

            if (userIdClaim == null || userRoleClaim == null)
                return Results.Unauthorized();

            string role = userRoleClaim.Value;
            string loggedInUserId = userIdClaim.Value;

            if (string.IsNullOrEmpty(role) || string.IsNullOrEmpty(loggedInUserId))
                return Results.Unauthorized();

            var session = await db.Sessions.FindAsync(sessionId);
            if (session == null) return Results.NotFound();

            if (role != "Admin" && session.GameMasterId.ToString() != loggedInUserId) return Results.Unauthorized();

            db.Sessions.Remove(session);
            await db.SaveChangesAsync();

            return Results.Ok();
        })
        .WithName("DeleteSession")
        .WithDescription("Deletes a game session by ID.")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .RequireAuthorization("LoggedIn")
        .WithOpenApi();

        // Get sessions by userId
        app.MapGet("/api/sessions/by-user/{userId}", async (MyDbContext db, Guid userId) =>
        {
            var characterSessions = await db.Characters
                                            .Where(c => c.UserId == userId)
                                            .Select(c => c.SessionId)
                                            .Distinct()
                                            .ToListAsync();

            var sessions = await db.Sessions
                                   .Where(s => characterSessions.Contains(s.SessionId))
                                   .ToListAsync();

            return Results.Ok(sessions);
        })
        .WithName("GetSessionsByUserId")
        .WithDescription("Gets game sessions by user ID based on character associations.")
        .Produces<Session>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .RequireAuthorization("LoggedIn")
        .WithOpenApi();
    }
}
