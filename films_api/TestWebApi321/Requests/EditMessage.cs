using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TestWebApi321.Requests;

public class EditMessage
{
    [Required]
    public int id_Message { get; set; }
    public string? Content { get; set; }
    public IFormFile? Image { get; set; }
}