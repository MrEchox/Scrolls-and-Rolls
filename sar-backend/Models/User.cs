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


    [EnumDataType(typeof(UserRole), ErrorMessage = "Role must be either 'Admin', 'Game Master', or 'Player' enumeration.")]
    public UserRole Role { get; set; } // Admin, Game Master, Player
}