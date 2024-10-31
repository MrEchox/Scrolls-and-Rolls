using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

public static class MessageEndpoints
{
    public static void MapMessageEndpoints(this WebApplication app)
    {
        // Get all messages in a session
        app.MapGet("/sessions/{sessionId}/messages", async (MyDbContext db, Guid sessionId) =>
        {
            var messages = await db.Messages
                                 .Include(m => m.UserId)
                                 .Where(m => m.SessionId == sessionId)
                                 .ToListAsync();
            return Results.Ok(messages);
        })
        .WithName("GetAllMessagesInSession")
        .WithDescription("Gets all messages in a session.")
        .Produces<Message>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .RequireAuthorization("LoggedIn")
        .WithOpenApi();

        // Get all messages in a session from a specific user
        app.MapGet("/sessions/{sessionId}/messages/{userId}", async (MyDbContext db, Guid sessionId, Guid userId) =>
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
        .WithOpenApi();

        // Post new message
        app.MapPost("/sessions/{sessionId}/messages", async (MyDbContext db, Message message) =>
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(message);

            if (!Validator.TryValidateObject(message, validationContext, validationResults, true))
            {
                return Results.BadRequest(validationResults);
            }

            db.Messages.Add(message);
            await db.SaveChangesAsync();
            return Results.Created($"/sessions/{message.SessionId}/messages/{message.MessageId}", message);
        })
        .WithName("CreateMessage")
        .WithDescription("Creates a new message.")
        .Accepts<Message>("The message to create.")
        .Produces<Message>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .RequireAuthorization("LoggedIn")
        .WithOpenApi();
    }
}
