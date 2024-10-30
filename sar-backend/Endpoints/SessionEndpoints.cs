using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

public static class SessionEndpoints
{
    public static void MapSessionEndpoints(this WebApplication app)
    {
        // Get all sessions
        app.MapGet("/sessions", async (MyDbContext db) =>
        {
            var sessions = await db.Sessions
                                 .Include(s => s.Items)
                                 .ToListAsync();
            return Results.Ok(sessions);
        })
        .WithName("GetAllSessions")
        .WithDescription("Gets all game sessions.")
        .Produces<Session>(StatusCodes.Status200OK)
        .RequireAuthorization("Admin")
        .WithOpenApi();

        // Get specific session
        app.MapGet("/sessions/{sessionId}", async (MyDbContext db, Guid sessionId) =>
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
        app.MapPost("/sessions", async (MyDbContext db, Session s) =>
        {
            var session = new Session
            {
                SessionId = Guid.NewGuid(),
                SessionName = s.SessionName,
                GameMasterId = s.GameMasterId
            };

            db.Sessions.Add(session);
            await db.SaveChangesAsync();

            return Results.Created($"/sessions/{session.SessionId}", session);
        })
        .WithName("CreateSession")
        .WithDescription("Creates a new game session.")
        .Accepts<Session>("The session to create.")
        .Produces<Session>(StatusCodes.Status201Created)
        .RequireAuthorization("GameMaster")
        .WithOpenApi();

        // Update session
        app.MapPut("/sessions/{sessionId}", async (MyDbContext db, Guid sessionId, Session s) =>
        {
            var session = await db.Sessions.FindAsync(sessionId);
            if (session == null) return Results.NotFound();

            session.SessionName = s.SessionName;
            session.GameMasterId = s.GameMasterId;
            await db.SaveChangesAsync();

            return Results.Ok(session);
        })
        .WithName("UpdateSession")
        .WithDescription("Updates a game session by ID.")
        .Accepts<Session>("The session to update.")
        .Produces<Session>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .RequireAuthorization("Admin")
        .WithOpenApi();

        // Delete session
        app.MapDelete("/sessions/{sessionId}", async (MyDbContext db, Guid sessionId) =>
        {
            var session = await db.Sessions.FindAsync(sessionId);
            if (session == null) return Results.NotFound();

            db.Sessions.Remove(session);
            await db.SaveChangesAsync();

            return Results.Ok();
        })
        .WithName("DeleteSession")
        .WithDescription("Deletes a game session by ID.")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .RequireAuthorization(new[] {"Admin", "GameMaster"})
        .WithOpenApi();
    }
}
