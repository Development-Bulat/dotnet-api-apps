namespace TestWebApi321.Requests;

public class CreateEditFilm
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int id_Genre { get; set; }
    public DateTime ReleaseDate { get; set; } 
    public float Rating { get; set; }
    public string imageUrl { get; set; }
}