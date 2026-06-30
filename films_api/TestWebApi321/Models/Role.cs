using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestWebApi321.Models;

public class Role
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int id_Role { get; set; }
    public string Name { get; set; }
}

public enum UserRoles
{
    User = 1,
    Admin = 2
}