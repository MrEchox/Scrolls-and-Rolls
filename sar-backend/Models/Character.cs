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
    [Range(1, 30, ErrorMessage = "Stat must be between 1 and 30.")]
    public int Dexterity { get; set; }
    [Required]
    [Range(1, 30, ErrorMessage = "Stat must be between 1 and 30.")]
    public int Strength { get; set; }
    [Required]
    [Range(1, 30, ErrorMessage = "Stat must be between 1 and 30.")]
    public int Constitution { get; set; }
    [Required]
    [Range(1, 30, ErrorMessage = "Stat must be between 1 and 30.")]
    public int Intelligence { get; set; }
    [Required]
    [Range(1, 30, ErrorMessage = "Stat must be between 1 and 30.")]
    public int Wisdom { get; set; }
    [Required]
    [Range(1, 30, ErrorMessage = "Stat must be between 1 and 30.")]
    public int Charisma { get; set; }


    public List<Item> Items { get; set; } = new();
}
