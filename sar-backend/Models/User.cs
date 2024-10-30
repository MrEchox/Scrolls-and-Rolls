using System.ComponentModel.DataAnnotations;

public class User
{
    public Guid UserId { get; set; }

    [Required]
    [StringLength(20, MinimumLength = 3)]
    public string Username { get; set; }
    [Required]
    public string PasswordHash { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [RegularExpression(@"^(Admin|GameMaster|Player)$", ErrorMessage = "Role must be either 'Admin' or 'Player'.")]
    public string Role { get; set; } // Admin, Game Master, Player
}