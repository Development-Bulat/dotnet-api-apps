using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ar_Card_Connect_Api.Models;

public class UserLogin
{
    [Key]
    public Guid login_id { get; set; } = Guid.NewGuid();
    public Guid user_id { get; set; }
    [ForeignKey("user_id")]
    public UserProfile? User { get; set; }
    [Required, EmailAddress]
    public string email { get; set; }
    [Required]
    public string password { get; set; }

    
}