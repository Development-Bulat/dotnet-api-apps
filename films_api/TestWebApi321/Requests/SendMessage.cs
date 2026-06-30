using Microsoft.AspNetCore.Http;

namespace TestWebApi321.Requests;

public class SendMessage
{
    public string? Content { get; set; }
    public int? id_Sender { get; set; }
    public int? id_Film { get; set; }
    public int? id_Recipient { get; set; }
    public IFormFile? Image { get; set; }
}