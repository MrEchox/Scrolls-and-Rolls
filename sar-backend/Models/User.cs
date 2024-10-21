using System;
using System.ComponentModel.DataAnnotations;

public class User
{
    public Guid UserId { get; set; }

    [Required]
    public string Username { get; set; }
    [Required]
    public string PasswordHash { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string Role { get; set; } // Admin, Game Master, Player
}