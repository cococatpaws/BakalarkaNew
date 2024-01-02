using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webapi.Models
{
    public class Order_Book
    {
        [ForeignKey("OrderID")]
        [Column("id_order")]
        public int OrderId { get; set; }
        public Order Order { get; set; }

        [ForeignKey("BookId")]
        [Column("id_book")]
        public int BookId { get; set; }
        public Book Book { get; set; }

        [Column("book_price")]
        [Required]
        public double BookPrice { get; set; }
        [Column("quantity_ordered")]
        [Required]
        public int QuantityOrdered { get; set; }
    }
}
