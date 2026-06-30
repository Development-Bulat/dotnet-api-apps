using System.ComponentModel.DataAnnotations;

namespace TestWebApi321.Models;

public class Genre
{
    [Key]
    public int id_Genre { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}