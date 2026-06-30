using System.ComponentModel.DataAnnotations;

namespace Ar_Card_Connect_Api.Requests;

public class ChangePasswordRequest
{
    [MinLength(6), MaxLength(100)]
    public string oldPassword { get; set; }
    [MinLength(6), MaxLength(100)]
    public string newPassword { get; set; }    
}