using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace webapi.Models.ResponseModels
{
    public class Login
    {
        [MaxLength(50)]
        [Required]
        public string UserName { get; set; }
        [Column("password")]
        [Required]
        public string Password { get; set; }
    }
}
