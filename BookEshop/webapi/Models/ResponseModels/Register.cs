using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace webapi.Models.ResponseModels
{
    public class Register
    {
        //User
        [MaxLength(50)]
        [Required]
        public string UserName { get; set; }
        [Column("password")]
        [Required]
        public string Password { get; set; }

        //Personal info
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }
        [MaxLength(50)]
        [Required]
        public string Surname { get; set; }
        [MaxLength(50)]
        [Required]
        public string Email { get; set; }
        [MaxLength(13)]
        [Required]
        public string PhoneNumber { get; set; }

        //Address
        [MaxLength(30)]
        public string Country { get; set; }
        [MaxLength(50)]
        public string City { get; set; }
        [MaxLength(50)]
        public string Street { get; set; }
        [MaxLength(10)]
        public string AddressNumber { get; set; }
        [MaxLength(5)]
        public string PostCode { get; set; }
    }
}
