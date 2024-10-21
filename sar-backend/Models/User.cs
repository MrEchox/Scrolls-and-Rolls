using System;
using System.ComponentModel.DataAnnotations;

public class User
/// TO-DO: Add API methods to this mfer
/// Do a migration because there's changes in other models
/// Configure JWT in appsettings.json
/// Configure JWT in Program.cs
/// Create a auth controller
/// Generate JWT in the controller 
/// Protect routes with JWT with the [Authorize] attribute
/// Save the role in the JWT token
{
    public Guid UserId { get; set; }
    [Required]
    public string Username { get; set; }
    [Required]
    public string PasswordHash { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string Role { get; set; }
}