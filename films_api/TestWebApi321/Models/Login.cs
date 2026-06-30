using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestWebApi321.Models
{
    public class Login
    {
        [Key]
        public int id_Login { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        [Required]
        [ForeignKey("User")]
        public int User_id { get; set; }
        public User User { get; set; }
    }
}
