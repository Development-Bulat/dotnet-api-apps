using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestWebApi321.Models;

public class ChatMessage
{
    [Key]
    public int id_Message { get; set; }
    public string? Content { get; set; }
    public string? ImageUrl { get; set; }
    
    [Required]
    [ForeignKey("Sender")]
    public int id_Sender { get; set; }
    public User Sender { get; set; }
    
    [ForeignKey("Film")]
    public int? id_Film { get; set; }
    public Film? Film { get; set; }

    [ForeignKey("Recipient")]
    public int? id_Recipient { get; set; }
    public User? Recipient { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}