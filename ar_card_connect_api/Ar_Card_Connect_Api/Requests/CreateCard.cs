using System.ComponentModel.DataAnnotations;

namespace Ar_Card_Connect_Api.Requests;

public class CreateCard
{
    [Required]
    [MinLength(1), MaxLength(255)]
    public string full_name { get; set; }
    [Phone]
    public string phone { get; set; }
    [EmailAddress, Required]
    [MinLength(3), MaxLength(255)]
    public string email { get; set; }
    [MaxLength(200)]
    public string? position { get; set; }
    [MaxLength(500)]
    public string? description { get; set; }
    public IFormFile? model_file { get; set; }
    public IFormFile? image_file { get; set; }
    [MinLength(1), MaxLength(255)]
    public string? template { get; set; }
    [MaxLength(1000)]
    public string? social_links { get; set; }
} 