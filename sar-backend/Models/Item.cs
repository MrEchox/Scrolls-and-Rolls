using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class Item
{
    public Guid ItemId { get; set; }
    [Required]
    [StringLength(20, MinimumLength = 3, ErrorMessage = "Item name must be between 3 and 20 characters.")]
    public string Name { get; set; }
    [Required]
    [StringLength(500, ErrorMessage = "Item name must be between 3 and 20 characters.")]

    public string Description { get; set; }

    // Modifiers
    [Range(0, 10, ErrorMessage = "Stat must be between 0 and 10.")]
    public int ModifiedDexterity { get; set; }
    [Range(0, 10, ErrorMessage = "Stat must be between 0 and 10.")]
    public int ModifiedStrength { get; set; }
    [Range(0, 10, ErrorMessage = "Stat must be between 0 and 10.")]
    public int ModifiedConstitution { get; set; }
    [Range(0, 10, ErrorMessage = "Stat must be between 0 and 10.")]
    public int ModifiedIntelligence { get; set; }
    [Range(0, 10, ErrorMessage = "Stat must be between 0 and 10.")]
    public int ModifiedWisdom { get; set; }
    [Range(0, 10, ErrorMessage = "Stat must be between 0 and 10.")]
    public int ModifiedCharisma { get; set; }

    public Guid SessionId { get; set; }
    public Guid? CharacterId { get; set; }

    // Nav property
    [JsonIgnore]
    public Session Session { get; set; }
}