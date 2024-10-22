using System.ComponentModel.DataAnnotations;

public class Session
{
    public Guid SessionId { get; set; }
    [Required]
    public Guid GameMasterId { get; set; }
    [Required]

    [StringLength(20, MinimumLength = 3, ErrorMessage = "Session name must be between 3 and 20 characters.")]
    public string SessionName { get; set; }
    public List<Item> Items { get; set; } = new();

}