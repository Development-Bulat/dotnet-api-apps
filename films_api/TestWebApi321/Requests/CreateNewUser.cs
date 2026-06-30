namespace TestWebApi321.Requests;
using System.Text.Json.Serialization;
    public class CreateNewUser
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int SelectedRoleId { get; set; }
    }

