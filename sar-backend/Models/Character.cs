using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class Character
{
    public Guid CharacterId { get; set; }
    public Guid SessionId { get; set; }

    [Required]
    public Guid UserId { get; set; }


    // Character Info
    [Required]
    [StringLength(500, ErrorMessage = "Character biography must be up to 500 characters.")]
    public string Biography { get; set; }
    [Required]
    public bool IsNpc { get; set; }
    [Required]
    [StringLength(20, MinimumLength = 3, ErrorMessage = "Character name must be between 3 and 20 characters.")]
    public string Name { get; set; }
    public int Gold { get; set; } = 0;


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
