using System.ComponentModel.DataAnnotations;

namespace Ar_Card_Connect_Api.Requests;

public class UpdateProfile
{
    [MaxLength(100)]
    public string? first_name { get; set; }
    [MaxLength(100)]
    public string? last_name { get; set; }
    [MaxLength(100)]
    public string? surname { get; set; }
    [Phone, MaxLength(11)]
    public string? phone { get; set; }
    public string? avatar_url { get; set; }
    [EmailAddress, MaxLength(255)]
    public string? email { get; set; }
    [MaxLength(100)]
    public string? password { get; set; }
}