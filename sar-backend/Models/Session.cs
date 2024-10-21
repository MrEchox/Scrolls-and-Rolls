using System.ComponentModel.DataAnnotations;

public class Session
{
    public Guid SessionId { get; set; }
    [Required]
    public Guid GameMasterId { get; set; }
    [Required]
    public string SessionName { get; set; }
    public List<Item> Items { get; set; } = new();

}