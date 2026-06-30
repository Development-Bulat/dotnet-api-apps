using System.Text.Json.Serialization;
namespace TestWebApi321.Requests;

public class UpdateUser
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Email { get; set; }
    public string? Password { get; set; }
    public int id_Role { get; set; }
}