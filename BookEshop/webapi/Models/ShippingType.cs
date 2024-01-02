using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using webapi.Enums;

namespace webapi.Models
{
    //[Table("shipping_type")]
    public class ShippingType
    {
        [Key]
        [Column("id_shipping")]
        public int ShippingId { get; set; }
        [Column("shipping_type")]
        [MaxLength(20)]
        [EnumDataType(typeof(webapi.Enums.ShippingType))]
        [Required]
        public string? ShippingTypeVar { get; set; }
        [Column("shipping_cost")]
        [Required]
        public double ShippingCost { get; set; }

        //Relationships
        public List<Order> Orders { get; set; }
    }
}
