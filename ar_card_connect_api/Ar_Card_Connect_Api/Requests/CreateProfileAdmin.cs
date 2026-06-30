using System.ComponentModel.DataAnnotations;

namespace Ar_Card_Connect_Api.Models;

public class CreateProfileAdmin
{
    [MinLength(1), MaxLength(100)]
    public string first_name { get; set; }
    [Phone, MaxLength(11)]
    public string? phone { get; set; }
    [MinLength(1), MaxLength(100)]
    public string last_name { get; set; }
    [MaxLength(100)]
    public string? surname { get; set; }
    public Guid role_id { get; set; }
    [Url]
    public string? avatar_url { get; set; }
    [EmailAddress, MinLength(3), MaxLength(255)]
    public string email { get; set; }
    [MinLength(6), MaxLength(100)]
    public string password { get; set; }
}