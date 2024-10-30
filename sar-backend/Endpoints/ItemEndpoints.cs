using Microsoft.EntityFrameworkCore;

public static class ItemEndpoints
{
    [Authorize]
    public static void MapItemEndpoints(this WebApplication app)
    {
        app.MapGet("/sessions/{sessionId}/items", (MyDbContext db, Guid sessionId) =>
        {
            var session = db.Sessions.Include(s => s.Items).FirstOrDefault(s => s.SessionId == sessionId);
            if (session == null) return Results.NotFound();

            return Results.Ok(session.Items);
        })
        .WithName("GetAllItems")
        .WithDescription("Gets all items in a specified session.")
        .Produces<List<Item>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .RequireAuthorization("LoggedIn")
        .WithOpenApi();

        // Get specific item
        app.MapGet("/sessions/{sessionId}/items/{itemId}", (MyDbContext db, Guid sessionId, Guid itemId) =>
        {
            var item = db.Items.FirstOrDefault(i => i.ItemId == itemId && i.SessionId == sessionId);
            if (item == null) return Results.NotFound();

            return Results.Ok(item);
        })
        .WithName("GetItemById")
        .WithDescription("Gets item by ID from specified session.")
        .Produces(StatusCodes.Status404NotFound)
        .Produces<Item>(StatusCodes.Status200OK)
        .RequireAuthorization("LoggedIn")
        .WithOpenApi();

        // Create item
        app.MapPost("/sessions/{sessionId}/items", (MyDbContext db, Guid sessionId, Item item) =>
        {
            var session = db.Sessions.FirstOrDefault(s => s.SessionId == sessionId);
            if (session == null) return Results.NotFound();
            item.ItemId = Guid.NewGuid();
            item.SessionId = sessionId;
            item.Session = session;

            db.Items.Add(item);
            db.SaveChanges();


            return Results.Created($"/sessions/{sessionId}/items/{item.ItemId}", item);
        })
        .WithName("CreateItem")
        .WithDescription("Creates a new item for specified session.")
        .Produces<Item>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status404NotFound)
        .RequireAuthorization(new[] {"Admin", "GameMaster"})
        .WithOpenApi();

        // Update item
        app.MapPut("/sessions/{sessionId}/items/{itemId}", (MyDbContext db, Guid sessionId, Guid itemId, Item item) =>
        {
            if (item == null) return Results.BadRequest("Missing item info in body.");

            var existingItem = db.Items.FirstOrDefault(s => s.SessionId == sessionId && s.ItemId == itemId);

            if (existingItem == null) return Results.NotFound("Item not found.");

            existingItem.Name = item.Name;
            existingItem.Description = item.Description;

            existingItem.ModifiedDexterity = item.ModifiedDexterity;
            existingItem.ModifiedStrength = item.ModifiedStrength;
            existingItem.ModifiedConstitution = item.ModifiedConstitution;
            existingItem.ModifiedIntelligence = item.ModifiedIntelligence;
            existingItem.ModifiedWisdom = item.ModifiedWisdom;
            existingItem.ModifiedCharisma = item.ModifiedCharisma;

            db.Items.Update(existingItem);
            db.SaveChanges();

            return Results.Ok(existingItem);
        })
        .WithName("UpdateItem")
        .WithDescription("Updates an item for specified session.")
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces<Item>(StatusCodes.Status200OK)
        .RequireAuthorization(new[] {"Admin", "GameMaster"})
        .WithOpenApi();

        // Delete item
        app.MapDelete("/sessions/{sessionId}/items/{itemId}", (MyDbContext db, Guid sessionId, Guid itemId) =>
        {
            var item = db.Items
                        .FirstOrDefault(i => i.SessionId == sessionId && i.ItemId == itemId);

            if (item == null) return Results.NotFound("Item not found.");

            if (item.CharacterId != null)
            {
                var character = db.Characters.FirstOrDefault(c => c.CharacterId == item.CharacterId);
                if (character == null) return Results.NotFound("Character not found.");

                character.Strength -= item.ModifiedStrength;
                character.Dexterity -= item.ModifiedDexterity;
                character.Constitution -= item.ModifiedConstitution;
                character.Intelligence -= item.ModifiedIntelligence;
                character.Wisdom -= item.ModifiedWisdom;
                character.Charisma -= item.ModifiedCharisma;
            }

            db.Items.Remove(item);
            db.SaveChanges();

            return Results.Ok();
        })
        .WithName("DeleteItem")
        .WithDescription("Deletes an item by ID for specified session.")
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status200OK)
        .RequireAuthorization(new[] {"Admin", "GameMaster"})
        .WithOpenApi();

        // Assign item to character
        app.MapPost("/sessions/{sessionId}/items/{itemId}/assign/{characterId}", (MyDbContext db, Guid sessionId, Guid itemId, Guid characterId) =>
        {
            var item = db.Items.FirstOrDefault(i => i.ItemId == itemId && i.SessionId == sessionId);
            if (item == null) return Results.NotFound("Item not found.");

            // Find the character (to assign the item to)
            var character = db.Characters.FirstOrDefault(c => c.CharacterId == characterId && c.SessionId == sessionId);
            if (character == null) return Results.NotFound("Character not found.");

            // Assign the item to the character
            item.CharacterId = character.CharacterId;

            character.Strength += item.ModifiedStrength;
            character.Dexterity += item.ModifiedDexterity;
            character.Constitution += item.ModifiedConstitution;
            character.Intelligence += item.ModifiedIntelligence;
            character.Wisdom += item.ModifiedWisdom;
            character.Charisma += item.ModifiedCharisma;

            character.Items.Add(item);

            db.SaveChanges();

            return Results.Ok(item);
        })
        .WithName("AssignItemToCharacter")
        .WithDescription("Assigns an item to a character in the same (specified) session.")
        .Produces(StatusCodes.Status404NotFound)
        .Produces<Item>(StatusCodes.Status200OK)
        .RequireAuthorization(new[] {"Admin", "GameMaster"})
        .WithOpenApi();

        // Items for characters ------------------------------------------------

        // Get all items for character
        app.MapGet("/sessions/{sessionId}/characters/{characterId}/items", (MyDbContext db, Guid sessionId, Guid characterId) =>
        {
            var character = db.Characters
                            .Include(c => c.Items)
                            .FirstOrDefault(c => c.SessionId == sessionId && c.CharacterId == characterId);

            if (character == null) return Results.NotFound();

            return Results.Ok(character.Items);
        })
        .WithName("GetAllItemsForCharacter")
        .WithDescription("Gets all items from specified character and session.")
        .Produces(StatusCodes.Status404NotFound)
        .Produces<List<Item>>(StatusCodes.Status200OK)
        .RequireAuthorization("LoggedIn")
        .WithOpenApi();

        // Assign item to character from character
        app.MapPost("/sessions/{sessionId}/characters/{characterId}/items/{itemId}/assign/{newCharacterId}", (MyDbContext db, Guid sessionId, Guid characterId, Guid itemId, Guid newCharacterId) =>
        {
            // Find the item assigned to the old character
            var item = db.Items.FirstOrDefault(i => i.ItemId == itemId && i.CharacterId == characterId);
            if (item == null) return Results.NotFound();

            // Find the old character (current owner of the item)
            var oldCharacter = db.Characters.FirstOrDefault(c => c.CharacterId == characterId && c.SessionId == sessionId);
            // Find the new character (to assign the item to)
            var newCharacter = db.Characters.FirstOrDefault(c => c.CharacterId == newCharacterId && c.SessionId == sessionId);

            if (oldCharacter == null || newCharacter == null) return Results.NotFound();

            // Revert item's stat effects from old character
            oldCharacter.Strength -= item.ModifiedStrength;
            oldCharacter.Dexterity -= item.ModifiedDexterity;
            oldCharacter.Constitution -= item.ModifiedConstitution;
            oldCharacter.Intelligence -= item.ModifiedIntelligence;
            oldCharacter.Wisdom -= item.ModifiedWisdom;
            oldCharacter.Charisma -= item.ModifiedCharisma;

            // Assign the item to the new character
            item.CharacterId = newCharacterId;

            newCharacter.Strength += item.ModifiedStrength;
            newCharacter.Dexterity += item.ModifiedDexterity;
            newCharacter.Constitution += item.ModifiedConstitution;
            newCharacter.Intelligence += item.ModifiedIntelligence;
            newCharacter.Wisdom += item.ModifiedWisdom;
            newCharacter.Charisma += item.ModifiedCharisma;

            db.SaveChanges();

            return Results.Ok(item);
        })
        .WithName("AssignCharacterItemToCharacter")
        .WithDescription("Assigns an item to a different character in the same (specified) session.")
        .Produces(StatusCodes.Status404NotFound)
        .Produces<Item>(StatusCodes.Status200OK)
        .RequireAuthorization(new[] {"Admin", "GameMaster"})
        .WithOpenApi();
    }
}

