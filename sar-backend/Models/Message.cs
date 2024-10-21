using System.ComponentModel.DataAnnotations;

public class Message
{
    public Guid MessageId { get; set; } = Guid.NewGuid();

    [Required]
    public Guid SessionId { get; set; }
    [Required]
    public Guid UserId { get; set; }


    [Required]
    public string MessageContent { get; set; }
    public DateTime TimeStamp { get; set; } = DateTime.Now;


    [Required]
    public string Channel { get; set; } // "Chat" or "Story"


    public Session session { get; set; }
    public User user { get; set; }
}