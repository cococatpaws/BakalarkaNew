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
        public List<ShippingAddress> ShippingAddresses { get; set; }
        public List<BillingAddress> BillingAddresses { get; set; }
        public List<Order> Orders { get; set; }

    }
}
