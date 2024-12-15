using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

public static class MessageEndpoints
{
    public static void MapMessageEndpoints(this WebApplication app)
    {
        // Get all messages in a session
        app.MapGet("/api/sessions/{sessionId}/messages", async (MyDbContext db, Guid sessionId, string channel) =>
        {
            var messages = await db.Messages
                                 .Where(m => m.SessionId == sessionId && m.Channel == channel)
                                 .ToListAsync();

            var characterNames = await db.Characters
                                         .Where(c => c.SessionId == sessionId)
                                         .ToDictionaryAsync(c => c.UserId, c => c.Name);

            var result = messages.Select(m => new
            {
                m.MessageId,
                m.SessionId,
                m.UserId,
                m.MessageContent,
                m.TimeStamp,
                m.Channel,
                CharacterName = characterNames.ContainsKey(m.UserId) ? characterNames[m.UserId] : "Guest"
            });

            return Results.Ok(result);
        })
        .WithName("GetAllMessagesInSession")
        .WithDescription("Gets all messages in a session.")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .RequireAuthorization("LoggedIn")
        .WithOpenApi();

        // Get all messages in a session from a specific user
        app.MapGet("/api/sessions/{sessionId}/messages/{userId}", async (MyDbContext db, Guid sessionId, Guid userId) =>
        {
            var messages = await db.Messages
                                 .Include(m => m.UserId)
                                 .Where(m => m.SessionId == sessionId && m.UserId == userId)
                                 .ToListAsync();
            return Results.Ok(messages);
        })
        .WithName("GetAllMessagesInSessionFromUser")
        .WithDescription("Gets all messages in a session from a specific user.")
        .Produces<Message>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .RequireAuthorization("LoggedIn")
        .WithOpenApi();

        // Post new message
        app.MapPost("/api/sessions/{sessionId}/messages", async (MyDbContext db, Message message) =>
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(message);

            if (!Validator.TryValidateObject(message, validationContext, validationResults, true))
            {
                return Results.BadRequest(validationResults);
            }

            db.Messages.Add(message);
            await db.SaveChangesAsync();

            return Results.Created($"/api/sessions/{message.SessionId}/messages/{message.MessageId}", message);
        })
        .WithName("CreateMessage")
        .WithDescription("Creates a new message.")
        .Produces<Message>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .RequireAuthorization("LoggedIn")
        .WithOpenApi();
    }
}
