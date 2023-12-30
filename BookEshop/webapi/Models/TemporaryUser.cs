using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webapi.Models
{
    [Table("temporary_user")]
    public class TemporaryUser
    {
        [Key, ForeignKey("UserTypeId")]
        [Column("id_user_type")]
        public int UserTypeIdTempUser { get; set; }
        //Relationships
       

        [ForeignKey("PersonalInfoId")]
        [Column("id_personal_info")]
        [Required]
        public int PersonalInfoIdTempUser { get; set; }

        public UserType UserType { get; set; }
        public PersonalInfo PersonalInfo { get; set; }

    }
}
