using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace webapi.Models
{
    [Table("personal_info")]
    public class PersonalInfo
    {
        [Key]
        [Column("id_personal_info")]
        public int PersonalInfoId { get; set; }
        [Column("name")]
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }
        [Column("surname")]
        [MaxLength(50)]
        [Required]
        public string Surname { get; set; }
        [Column("email")]
        [MaxLength(50)]
        [Required]
        public string Email { get; set; }
        [Column("phone_number")]
        [MaxLength(13)]
        [Required]
        public string PhoneNumber { get; set; }

        //Relationship

    }
}
