using System.ComponentModel.DataAnnotations;

public class Session
{
    public Guid SessionId { get; set; }
    [Required]
    public string GameMasterName { get; set; }
    public List<Item> Items { get; set; } = new();

}