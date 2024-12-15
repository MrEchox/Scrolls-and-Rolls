using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

public static class CharacterEndpoints
{
    public static void MapCharacterEndpoints(this WebApplication app)
    {
        // Get all characters
        app.MapGet("/api/sessions/{sessionId}/characters", (MyDbContext db, Guid sessionId) =>
        {
            var session = db.Sessions
                            .FirstOrDefault(s => s.SessionId == sessionId);

            if (session == null) return Results.NotFound();

            var characters = db.Characters.Where(s => s.SessionId == sessionId);

            return Results.Ok(characters.ToList());
        })
        .WithName("GetAllCharacters")
        .WithDescription("Gets all characters from specified session.")
        .Produces<Character>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .RequireAuthorization("GameMaster")
        .WithOpenApi();

        // Get session NPCs
        app.MapGet("/api/sessions/{sessionId}/npcs", (MyDbContext db, Guid sessionId) =>
        {
            var session = db.Sessions
                            .Include(c => c.Items)
                            .FirstOrDefault(s => s.SessionId == sessionId);

            if (session == null) return Results.NotFound();

            var npcs = db.Characters.Where(c => c.SessionId == sessionId).Where(c => c.IsNpc);

            return Results.Ok(npcs.ToList());
        })
        .WithName("GetAllNPCs")
        .WithDescription("Gets all NPCs (Non-Player characters) from specified session.")
        .Produces<Character>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .RequireAuthorization("LoggedIn")
        .WithOpenApi();

        // Get session PCs
        app.MapGet("/api/sessions/{sessionId}/pcs", (MyDbContext db, Guid sessionId) =>
        {
            var session = db.Sessions
                            .Include(c => c.Items)
                            .FirstOrDefault(s => s.SessionId == sessionId);

            if (session == null) return Results.NotFound();

            var pcs = db.Characters.Where(c => c.SessionId == sessionId).Where(c => !c.IsNpc);

            return Results.Ok(pcs.ToList());
        })
        .WithName("GetAllPCs")
        .WithDescription("Gets all PCs (Player characters) from specified session.")
        .Produces<Character>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .RequireAuthorization("LoggedIn")
        .WithOpenApi();

        // Get specific character
        app.MapGet("/api/sessions/{sessionId}/characters/{characterId}", (MyDbContext db, Guid sessionId, Guid characterId) =>
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
        app.MapPost("/api/sessions/{sessionId}/characters", (MyDbContext db, Guid sessionId, Character character) =>
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(character);

            if (!Validator.TryValidateObject(character, validationContext, validationResults, true))
            {
                return Results.BadRequest(validationResults);
            }

            var session = db.Sessions.FirstOrDefault(s => s.SessionId == sessionId);
            if (session == null) return Results.NotFound();

            character.CharacterId = Guid.NewGuid();
            character.SessionId = sessionId;

            db.Characters.Add(character);
            db.SaveChanges();

            return Results.Created($"/api/sessions/{sessionId}/characters/{character.CharacterId}", character);
        })
        .WithName("CreateCharacter")
        .WithDescription("Creates a new character for specific session.")
        .Produces<Character>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound)
        .RequireAuthorization("LoggedIn")
        .WithOpenApi();

        // Update character
        app.MapPut("/api/sessions/{sessionId}/characters/{characterId}", (MyDbContext db, Guid sessionId, Guid characterId, Character character) =>
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(character);

            if (!Validator.TryValidateObject(character, validationContext, validationResults, true))
            {
                return Results.BadRequest(validationResults);
            }

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
        .RequireAuthorization("Admin")
        .WithOpenApi();

        // Delete character
        app.MapDelete("/api/sessions/{sessionId}/characters/{characterId}", (MyDbContext db, Guid sessionId, Guid characterId) =>
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
        .RequireAuthorization("GameMaster")
        .WithOpenApi();
    }
}