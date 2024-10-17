using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class Character
{
    public Guid CharacterId { get; set; }
    public Guid SessionId { get; set; }

    [Required]
    public string Biography { get; set; }

    [Required]
    public bool IsNpc { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
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
