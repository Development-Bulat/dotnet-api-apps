using System.ComponentModel.DataAnnotations;

namespace Ar_Card_Connect_Api.Requests;

public class Auth
{
    [Required]
    [EmailAddress,  MinLength(3), MaxLength(255)]
    public string email { get; set; }
    [Required]
    [MinLength(6), MaxLength(100)]
    public string password { get; set; }
}