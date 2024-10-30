using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

public static class CharacterEndpoints
{
    public static void MapCharacterEndpoints(this WebApplication app)
    {
        // Get all characters
        app.MapGet("/sessions/{sessionId}/characters", (MyDbContext db, Guid sessionId) =>
        {
            var session = db.Sessions
                            .Include(c => c.Items)
                            .FirstOrDefault(s => s.SessionId == sessionId);

            if (session == null) return Results.NotFound();

            var characters = db.Characters.Where(s => s.SessionId == sessionId);

            return Results.Ok(characters.ToList());
        })
        .WithName("GetAllCharacters")
        .WithDescription("Gets all characters from specified session.")
        .Produces<Character>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .RequireAuthorization("Admin")
        .WithOpenApi();

        // Get specific character
        app.MapGet("/sessions/{sessionId}/characters/{characterId}", (MyDbContext db, Guid sessionId, Guid characterId) =>
        {
            var character = db.Characters
                            .Include(c => c.Items)
                            .FirstOrDefault(c => c.SessionId == sessionId && c.CharacterId == characterId);

            if (character == null) return Results.NotFound();

            return Results.Ok(character);
        })
        .WithName("GetCharacterById")
        .WithDescription("Gets character by ID from specified session.")
        .Produces<Character>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .RequireAuthorization("LoggedIn")
        .WithOpenApi();

        // Create character
        app.MapPost("/sessions/{sessionId}/characters", (MyDbContext db, Guid sessionId, Character character) =>
        {
            var session = db.Sessions.FirstOrDefault(s => s.SessionId == sessionId);
            if (session == null) return Results.NotFound();

            character.CharacterId = Guid.NewGuid();
            character.SessionId = sessionId;

            db.Characters.Add(character);
            db.SaveChanges();

            return Results.Created($"/sessions/{sessionId}/characters/{character.CharacterId}", character);
        })
        .WithName("CreateCharacter")
        .WithDescription("Creates a new character for specific session.")
        .Produces<Character>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status404NotFound)
        .RequireAuthorization("LoggedIn")
        .WithOpenApi();

        // Update character
        app.MapPut("/sessions/{sessionId}/characters/{characterId}", (MyDbContext db, Guid sessionId, Guid characterId, Character character) =>
        {
            if (character == null) return Results.BadRequest("Missing character info in body.");

            var existingCharacter = db.Characters.FirstOrDefault(c => c.SessionId == sessionId && c.CharacterId == characterId);

            if (existingCharacter == null) return Results.NotFound("Character not found.");

            existingCharacter.Biography = character.Biography;
            existingCharacter.Gold = character.Gold;

            existingCharacter.Dexterity = character.Dexterity;
            existingCharacter.Strength = character.Strength;
            existingCharacter.Constitution = character.Constitution;
            existingCharacter.Intelligence = character.Intelligence;
            existingCharacter.Wisdom = character.Wisdom;
            existingCharacter.Charisma = character.Charisma;

            existingCharacter.Items = character.Items;

            db.Characters.Update(existingCharacter);
            db.SaveChanges();

            return Results.Ok(existingCharacter);
        })
        .WithName("UpdateCharacter")
        .WithDescription("Updates a character by ID for specific session.")
        .Produces<Character>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound)
        .RequireAuthorization("LoggedIn")
        .WithOpenApi();

        // Delete character
        app.MapDelete("/sessions/{sessionId}/characters/{characterId}", (MyDbContext db, Guid sessionId, Guid characterId) =>
        {
            var character = db.Characters
                            .FirstOrDefault(c => c.SessionId == sessionId && c.CharacterId == characterId);

            if (character == null) return Results.NotFound();

            db.Characters.Remove(character);
            db.SaveChanges();

            return Results.Ok(character);
        })
        .WithName("DeleteCharacter")
        .WithDescription("Deletes a character by ID for specific session.")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .RequireAuthorization("LoggedIn")
        .WithOpenApi();
    }
}