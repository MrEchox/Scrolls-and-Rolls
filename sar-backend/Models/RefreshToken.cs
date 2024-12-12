using System.ComponentModel.DataAnnotations;

public class RefreshToken
{
    public Guid RefreshTokenId { get; set; }
    public Guid UserId { get; set; }
    public DateTime Expires { get; set; }

}