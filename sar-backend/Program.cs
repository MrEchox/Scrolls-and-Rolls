using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure Entity Framework and connect to Azure SQL DB
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// API Methods -------------------------------------------------------------------------------------------

// Session -----------------------------------------------------
// Get all sessions
app.MapGet("/sessions", async (MyDbContext db) =>
{
    var sessions = await db.Sessions.ToListAsync();
    return Results.Ok(sessions);
})
.WithName("GetAllSessions")
.WithDescription("Gets all game sessions.")
.Produces<Session>(StatusCodes.Status200OK)
.WithOpenApi();

// Get specific session
app.MapGet("/sessions/{sessionId}", async (MyDbContext db, Guid sessionId) =>
{
    var session = await db.Sessions
                        .FirstOrDefaultAsync(s => s.SessionId == sessionId);

    if (session == null) return Results.NotFound();

    return Results.Ok(session);
})
.WithName("GetSessionById")
.WithDescription("Gets game session by ID.")
.Produces<Session>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound)
.WithOpenApi();

// Create session
app.MapPost("/sessions", async (MyDbContext db, Session s) =>
{
    var session = new Session
    {
        SessionId = Guid.NewGuid(),
        GameMasterName = s.GameMasterName
    };

    db.Sessions.Add(session);
    await db.SaveChangesAsync();

    return Results.Created($"/sessions/{session.SessionId}", session);
})
.WithName("CreateSession")
.WithDescription("Creates a new game session.")
.Produces<Session>(StatusCodes.Status201Created)
.WithOpenApi();

// Update session
app.MapPut("/sessions/{sessionId}", async (MyDbContext db, Guid sessionId, Session s) =>
{
    var session = await db.Sessions.FindAsync(sessionId);
    if (session == null) return Results.NotFound();

    session.GameMasterName = s.GameMasterName;
    await db.SaveChangesAsync();

    return Results.Ok(session);
})
.WithName("UpdateSession")
.WithDescription("Updates a game session by ID.")
.Produces<Session>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound)
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
.WithOpenApi();

// -----------------------------------------------------

// Character ------------------------------------------------
// Get all characters
app.MapGet("/sessions/{sessionId}/characters", (MyDbContext db, Guid sessionId) =>
{
    var session = db.Sessions
                    .FirstOrDefault(s => s.SessionId == sessionId);

    if (session == null) return Results.NotFound();

    var characters = db.Characters.Where(s  => s.SessionId == sessionId);

    return Results.Ok(characters.ToList());
})
.WithName("GetAllCharacters")
.WithDescription("Gets all characters from specified session.")
.Produces<Character>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound)
.WithOpenApi();

// Get specific character
app.MapGet("/sessions/{sessionId}/characters/{characterId}", (MyDbContext db, Guid sessionId, Guid characterId) =>
{
    var character = db.Characters
                    .FirstOrDefault(c => c.SessionId == sessionId && c.CharacterId == characterId);

    if (character == null) return Results.NotFound();

    return Results.Ok(character);
})
.WithName("GetCharacterById")
.WithDescription("Gets character by ID from specified session.")
.Produces<Character>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound)
.WithOpenApi();

// Create character
app.MapPost("/sessions/{sessionId}/characters", (MyDbContext db, Guid sessionId, Character character) =>
{
    var session = db.Sessions.FirstOrDefault(s => s.SessionId == sessionId);
    if (session == null) return Results.NotFound();

    character.CharacterId = Guid.NewGuid();

    db.Characters.Add(character);
    db.SaveChanges();

    return Results.Created($"/sessions/{sessionId}/characters/{character.CharacterId}", character);
})
.WithName("CreateCharacter")
.WithDescription("Creates a new character for specific session.")
.Produces<Character>(StatusCodes.Status201Created)
.Produces(StatusCodes.Status404NotFound)
.WithOpenApi();

// Update character
app.MapPut("/sessions/{sessionId}/characters/{characterId}", (MyDbContext db, Guid sessionId, Guid characterId, Character character) =>
{
    if (characterId != character.CharacterId) return Results.BadRequest("CharacterId in URL does not match CharacterId in body.");
    if (character == null) return Results.BadRequest("Missing character info in body.");

    var existingCharacter = db.Characters
                            .FirstOrDefault(c => c.SessionId == sessionId && c.CharacterId == characterId);

    if (existingCharacter == null) return Results.NotFound("Character not found.");

    db.Characters.Update(character);
    db.SaveChanges();

    return Results.Ok(character);
})
.WithName("UpdateCharacter")
.WithDescription("Updates a character by ID for specific session.")
.Produces<Character>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status400BadRequest)
.Produces(StatusCodes.Status404NotFound)
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
.WithOpenApi();

// ------------------------------------------------

// Item ------------------------------------------------
// Get all items
app.MapGet("/sessions/{sessionId}/characters/{characterId}/items", (MyDbContext db, Guid sessionId, Guid characterId) =>
{
    var character = db.Characters
                    .Include(c => c.Items)
                    .FirstOrDefault(c => c.SessionId == sessionId && c.CharacterId == characterId);

    if (character == null) return Results.NotFound();

    return Results.Ok(character.Items);
})
.WithName("GetAllItems")
.WithDescription("Gets all items from specified character and session.")
.Produces(StatusCodes.Status404NotFound)
.Produces<List<Item>>(StatusCodes.Status200OK)
.WithOpenApi();

// Get specific item
app.MapGet("/sessions/{sessionId}/characters/{characterId}/items/{itemId}", (MyDbContext db, Guid sessionId, Guid characterId, Guid itemId) =>
{
    var item = db.Items
                .FirstOrDefault(i => i.CharacterId == characterId && i.ItemId == itemId);

    if (item == null) return Results.NotFound();

    return Results.Ok(item);
})
.WithName("GetItemById")
.WithDescription("Gets item by ID from specified character and session.")
.Produces(StatusCodes.Status404NotFound)
.Produces<Item>(StatusCodes.Status200OK)
.WithOpenApi();

// Create item
app.MapPost("/sessions/{sessionId}/characters/{characterId}/items", (MyDbContext db, Guid sessionId, Guid characterId, Item item) =>
{
    var character = db.Characters.FirstOrDefault(c => c.SessionId == sessionId && c.CharacterId == characterId);
    if (character == null) return Results.NotFound();
    item.ItemId = Guid.NewGuid();

    db.Items.Add(item);
    db.SaveChanges();


    return Results.Created($"/sessions/{sessionId}/characters/{characterId}/items/{item.ItemId}", item);
})
.WithName("CreateItem")
.WithDescription("Creates a new item for specified character and session.")
.Produces<Item>(StatusCodes.Status201Created)
.Produces(StatusCodes.Status404NotFound)
.WithOpenApi();

// Update item
app.MapPut("/sessions/{sessionId}/characters/{characterId}/items/{itemId}", (MyDbContext db, Guid sessionId, Guid characterId, Guid itemId, Item item) =>
{
    if (itemId != item.ItemId) return Results.BadRequest("ItemId in URL does not match ItemId in body.");
    if (item == null) return Results.BadRequest("Missing item info in body.");

    var existingItem = db.Items
                            .FirstOrDefault(c => c.CharacterId == characterId && c.ItemId == itemId);

    if (existingItem == null) return Results.NotFound("Item not found.");

    db.Items.Update(item);
    db.SaveChanges();

    return Results.Ok(item);
})
.WithName("UpdateItem")
.WithDescription("Updates an item for specified character and session.")
.Produces(StatusCodes.Status404NotFound)
.Produces(StatusCodes.Status400BadRequest)
.Produces<Item>(StatusCodes.Status200OK)
.WithOpenApi();

// Delete item
app.MapDelete("/sessions/{sessionId}/characters/{characterId}/items/{itemId}", (MyDbContext db, Guid sessionId, Guid characterId, Guid itemId) =>
{
    var item = db.Items
                .FirstOrDefault(i => i.CharacterId == characterId && i.ItemId == itemId);

    if (item == null) return Results.NotFound();

    db.Items.Remove(item);
    db.SaveChanges();

    return Results.Ok();
})
.WithName("DeleteItem")
.WithDescription("Deletes an item by ID for specified character and session.")
.Produces(StatusCodes.Status400BadRequest)
.Produces(StatusCodes.Status200OK)
.WithOpenApi();

// Assign item to character
app.MapPost("/sessions/{sessionId}/characters/{characterId}/items/{itemId}/assign", (MyDbContext db, Guid sessionId, Guid characterId, Guid itemId, Guid newCharacterId) =>
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
.WithName("AssignItemToCharacter")
.WithDescription("Assigns an item to a different character in the same (specified) session.")
.Produces(StatusCodes.Status404NotFound)
.Produces<Item>(StatusCodes.Status200OK)
.WithOpenApi();

// ------------------------------------------------

app.Run();

public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) {}

    public DbSet<Session> Sessions { get; set; }
    public DbSet<Character> Characters { get; set; }
    public DbSet<Item> Items { get; set; }
}

public class Session
{
    public Guid SessionId { get; set; }
    [Required]
    public string GameMasterName { get; set; }
}

public class Character
{
    [JsonIgnore]
    public Guid CharacterId { get; set; }
    [Required]
    public string Biography { get; set; }
    [Required]
    public bool IsNpc { get; set; }  // True if created by GM
    [Required]
    public Guid SessionId { get; set; }

    public string Name { get; set; }


    // Stats
    [Required]
    public int Dexterity { get; set; }
    [Required]
    public int Strength { get; set; }
    [Required]
    public int Constitution { get; set; }
    [Required]
    public int Intelligence { get; set; }
    [Required]
    public int Wisdom { get; set; }
    [Required]
    public int Charisma { get; set; }

    public List<Item> Items { get; set; } = new();
}

public class Item
{
    public Guid ItemId { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }

    // Modifiers
    public int ModifiedDexterity { get; set; }
    public int ModifiedStrength { get; set; }
    public int ModifiedConstitution { get; set; }
    public int ModifiedIntelligence { get; set; }
    public int ModifiedWisdom { get; set; }
    public int ModifiedCharisma { get; set; }

    [Required]
    public Guid CharacterId { get; set; }
}
