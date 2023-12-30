using Microsoft.EntityFrameworkCore.Migrations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using webapi.Enums;

namespace webapi.Models
{
    [Table("user")]
    public class User
    {
        [Key, ForeignKey("UserTypeId")]
        [Column("id_user_type")]
        public int UserTypeIdUser { get; set; }
        [ForeignKey("PersonalInfoId")]
        [Column("id_personal_info")]
        [Required]
        public int PersonalInfoIdUser { get; set; }
        [Column("username")]
        [MaxLength(50)]
        [Required]
        public string UserName { get; set; }
        [Column("profile_picture_url")]
        [MaxLength(255)]
        public string ProfilePictureUrl { get; set; }
        [Column("password")]
        [Required]
        public string Password { get; set; }
        [Column("role")]
        [Required]
        [MaxLength(5)]
        [EnumDataType(typeof(Role))]
        public string Role { get; set; }



        //Relationships - refrences
        public UserType UserType { get; set; }
        public PersonalInfo PersonalInfo { get; set; }
    }
}
