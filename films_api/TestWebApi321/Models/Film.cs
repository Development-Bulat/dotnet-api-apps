using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestWebApi321.Models;

public class Film
{
    [Key]
    public int id_Film { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    [Required]
    [ForeignKey("Genre")]
    public int id_Genre { get; set; }
    public Genre Genre { get; set; }
    public DateTime ReleaseDate { get; set; } = DateTime.UtcNow;
    public float Rating { get; set; }
    public string imageUrl { get; set; }
    
}