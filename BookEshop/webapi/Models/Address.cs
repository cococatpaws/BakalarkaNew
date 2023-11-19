using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webapi.Models
{
    public class Address
    {
        [Key]
        [Column("id_address")]
        public int AddressId { get; set; }
        [Column("country")]
        [MaxLength(30)]
        public string Country { get; set; }
        [Column("city")]
        [MaxLength(50)]
        public string City { get; set; }
        [Column("address_number")]
        [MaxLength(10)]
        public string AddressNumber { get; set; }
        [Column("postcode")]
        [MaxLength(5)]
        public string PostCode { get; set; }


    }
}
