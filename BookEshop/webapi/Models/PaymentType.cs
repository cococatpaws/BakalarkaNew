using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace webapi.Models
{
    //[Table("payment_type")]
    public class PaymentType
    {
        [Key]
        [Column("id_payment")]
        public int PaymentId { get; set; }
        [Column("payment_type")]
        [MaxLength(20)]
        [EnumDataType(typeof(webapi.Enums.PaymentType))]
        [Required]
        public string? PaymentTypeVar { get; set; }
        [Column("additional_cost")]
        [Required]
        public double AdditionalCost { get; set; }

        //Relationships
        public List<Order> Orders { get; set; }
    }
}
