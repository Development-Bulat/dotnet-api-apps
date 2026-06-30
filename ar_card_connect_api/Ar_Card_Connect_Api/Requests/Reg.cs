using System.ComponentModel.DataAnnotations;

namespace Ar_Card_Connect_Api.Requests;

public class Reg
{
    [Required]
    [MinLength(1), MaxLength(100)]
    public string last_name { get; set; }
    [Required]
    [MinLength(1), MaxLength(100)]
    public string first_name { get; set; }
    [MaxLength(100)]
    public string? surname { get; set; }
    [Required]
    [EmailAddress,  MinLength(3), MaxLength(255)]
    public string email { get; set; }
    [Required]
    [MinLength(6), MaxLength(100)]
    public string password { get; set; }
}