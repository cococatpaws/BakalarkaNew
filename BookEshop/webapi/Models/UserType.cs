using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webapi.Models
{
    [Table("user_type")]
    public class UserType
    {
        [Key]
        [Column("id_user_type")]
        public int UserTypeId { get; set; }

        // Relationships
        [ForeignKey("ShippingAddressId")]
        [Column("id_shipping_adress")]
        public int ShippingAddressIdUser { get; set; }

        [ForeignKey("BillingAddressId")]
        [Column("id_billing_address")]
        public int BillingAddressIdUser { get; set; }

        public ShippingAddress ShippingAddress { get; set; }
        public BillingAddress BillingAddress { get; set; }

    }
}
