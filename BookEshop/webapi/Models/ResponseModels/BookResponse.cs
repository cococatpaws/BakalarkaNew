using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using webapi.Enums;

namespace webapi.Models
{
    public class BookResponse
    {
        public int BookId { get; set; }
        [MaxLength(60)]
        public string Title { get; set; }
        [MaxLength(5000)]
        public string? Description { get; set; }
        [Column("quantity_in_stock")]
        public int QuantityInStock { get; set; }
        [MaxLength(255)]
        public string CoverImageURL { get; set; }
        [MaxLength(20)]
        [EnumDataType(typeof(Genre))]
        public string? Genre { get; set; }
        public double Price { get; set; }
        [MaxLength(50)]
        public string? Publisher { get; set; }
        public int? NumberOfPages { get; set; }
        [MaxLength(15)]
        [EnumDataType(typeof(BookFormat))]
        public string BookFormat { get; set; }
        [Column("publication_date")]
        public DateTime PublicationDate { get; set; }
        [MaxLength(20)]
        [EnumDataType(typeof(BookLanguage))]
        public string BookLanguage { get; set; }
        public List<Author> BooksAuthors { get; set; }
        //public IFormFile? CoverImage { get; set; }
        public bool? DeleteImage { get; set; }
    }
}
