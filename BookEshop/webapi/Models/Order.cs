using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webapi.Models
{
    //[Table("order")]
    public class Order
    {
        [Key]
        [Column("id_order")]
        public int OrderID { get; set; }
        [Column("date_placed")]
        [DataType(DataType.Date)]
        public DateTime? DatePlaced { get; set; }
        [Column("order_status")]
        [EnumDataType(typeof(webapi.Enums.OrderStatus))]
        [Required]
        [MaxLength(20)]
        public string? OrderStatus { get; set; }
        [Column("order_type")]
        [EnumDataType(typeof(webapi.Enums.OrderType))]
        [Required]
        [MaxLength(20)]
        public string? OrderType { get; set; }
        [Column("order_details")]
        [MaxLength(200)]
        public string? OrderDetails { get; set; }


        //Relationships
        [ForeignKey("ShippingId")]
        [Column("id_shipping_type")]
        public int ShippingTypeId { get; set; }
        public ShippingType ShippingType { get; set; }

        [ForeignKey("PaymentId")]
        [Column("id_payment_type")]
        public int PaymentTypeId { get; set; }
        public PaymentType PaymentType { get; set; }

        [Column("id_user_type")]
        public int UserTypeId { get; set; }
        public UserType UserType { get; set; }

        public List<Order_Book> OrdersBooks { get; set; }


        [ForeignKey("ShippingAddressId")]
        [Column("id_shipping_address")]
        public int ShippingAddressIdO { get; set; }
        public ShippingAddress ShippingAddress  { get; set; }

        [ForeignKey("BillingAddressId")]
        [Column("id_billing_address")]
        public int BillingAddressIdO { get; set; }
        public BillingAddress BillingAddress { get; set; }
    }
}
