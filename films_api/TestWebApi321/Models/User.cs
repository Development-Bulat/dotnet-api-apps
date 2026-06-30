using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestWebApi321.Models
{
    public class User
    {
        [Key]
        public int id_User { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } 
        [Required]
        [ForeignKey("Role")]
        public int id_Role { get; set; } = 1;
        public Role Role { get; set; }
    }
}
