using System.ComponentModel.DataAnnotations;

public class DiceRollDto
{
    [Required(ErrorMessage = "Dice type is required.")]
    [RegularExpression(@"^d(100|[1-9][0-9]?)$", ErrorMessage = "Invalid dice type format. Use 'dX' where X is the number of sides, up to d100.")]
    public string DiceType { get; set; } // e.g., "d20"

    [Range(1, 100, ErrorMessage = "Number of dice must be between 1 and 100.")]
    public int NumberOfDice { get; set; } // Number of dice to roll

    public int Modifier { get; set; } // Modifier to add to the total
}
