using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ar_Card_Connect_Api.Models;

public class UserCard
{
    [Key]
    public Guid userCard_id { get; set; }
    public Guid user_id { get; set; }
    [ForeignKey("user_id")]
    public UserProfile? User { get; set; }
    [Required]
    public string full_name { get; set; }
    [Required]
    public string phone { get; set; }
    [Required]
    public string email { get; set; }
    public string? position { get; set; }
    public string? description { get; set; }
    public string? model_url { get; set; }
    public string? image_url { get; set; }
    public List<string>? social_links { get; set; }
    public string template { get; set; }
    [Required]
    public string marker_id { get; set; }
    public DateTime created_at { get; set; } = DateTime.UtcNow;
    public bool is_blocked { get; set; }
}