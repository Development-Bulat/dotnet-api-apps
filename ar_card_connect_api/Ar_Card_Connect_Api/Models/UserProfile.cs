using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ar_Card_Connect_Api.Models;

public class UserProfile
{
    [Key]
    public Guid user_id { get; set; }  = Guid.NewGuid();
    public Guid role_id { get; set; }
    [ForeignKey("role_id")]
    public Role? Role { get; set; }
    public string first_name { get; set; }
    public string last_name { get; set; }
    public string? surname { get; set; }
    public string? phone { get; set; }
    public string? avatar_url { get; set; }
    public virtual List<UserCard> Cards { get; set; } = new();
    public bool is_blocked { get; set; }
}