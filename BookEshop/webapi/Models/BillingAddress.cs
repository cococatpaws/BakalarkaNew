using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webapi.Models
{
    [Table("billing_address")]
    public class BillingAddress
    {
        [Key]
        [Column("id_billing_address")]
        public int BillingAddressId { get; set; }


        //Relatiosnhips
        [ForeignKey("AddressId")]
        [Column("id_address")]
        [Required]
        public int AddressIdB { get; set; }
        public Address Address { get; set; }
    }
}
