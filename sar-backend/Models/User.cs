using System.ComponentModel.DataAnnotations;

public class User
{
    public Guid UserId { get; set; }

    [Required]
    [StringLength(20, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 20 characters.")]
    public string Username { get; set; }
    [Required]
    public string PasswordHash { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [RegularExpression(@"^(Admin|GameMaster|Player)$", ErrorMessage = "Role must be either 'Admin', 'GameMaster' or 'Player'.")]
    public string Role { get; set; } // Admin, Game Master, Player
}