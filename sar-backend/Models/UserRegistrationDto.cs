using System.ComponentModel.DataAnnotations;

public class UserRegistrationDto
{
    [Required]
    [StringLength(20, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 20 characters.")]
    public string Username { get; set; }

    [Required]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,32}$", ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, and one number.")]
    [StringLength(32, MinimumLength = 8, ErrorMessage = "Password must be at least 8 and 32 characters.")]
    public string Password { get; set; }

    [Required]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string Email { get; set; }

    [Required]
    [RegularExpression(@"^(Admin|GameMaster|Player)$", ErrorMessage = "Role must be either 'Admin' or 'Player'.")]
    public string Role { get; set; }
}

