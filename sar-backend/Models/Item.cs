using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class Item
{
    public Guid ItemId { get; set; }
    [Required]
    [StringLength(20, MinimumLength = 3, ErrorMessage = "Session name must be between 3 and 20 characters.")]
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

    public Guid SessionId { get; set; }
    public Guid? CharacterId { get; set; }

    // Nav property
    [JsonIgnore]
    public Session Session { get; set; }
}