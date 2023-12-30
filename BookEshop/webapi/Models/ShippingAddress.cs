using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webapi.Models
{
    [Table("shipping_address")]
    public class ShippingAddress
    {
        [Key]
        [Column("id_shipping_address")]
        public int ShippingAddressId { get; set; }
        [Column("shipping_details")]
        [MaxLength(200)]
        public string ShippingDetails { get; set; }


        //Relationships
        [ForeignKey("AddressId")]
        [Column("id_address")]
        [Required]
        public int AddressIdS { get; set; }
        public Address Address { get; set; }

    }
}
