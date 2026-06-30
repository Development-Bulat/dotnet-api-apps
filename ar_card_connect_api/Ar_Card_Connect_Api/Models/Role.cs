using System.ComponentModel.DataAnnotations;

namespace Ar_Card_Connect_Api.Models;

public class Role
{
    [Key]
    public Guid role_id { get; set; } = Guid.NewGuid();
    [Required]
    public string role_name { get; set; }
}