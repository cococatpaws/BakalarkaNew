using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using webapi.Enums;

namespace webapi.Models
{
    [Table("book")]
    public class Book
    {
        [Key][Column("id_book")]
        public int BookId { get; set; }
        [Column("title")]
        [MaxLength(60)]
        public string Title { get; set; }
        [Column("description")]
        [MaxLength(5000)]
        public string? Description { get; set; }
        [Column("quantity_in_stock")]
        public int QuantityInStock { get; set; }

        [Column("cover_image_url")]
        [MaxLength(255)]
        public string CoverImageURL { get; set; }
        [Column("genre")]
        [MaxLength(20)]
        [EnumDataType(typeof(Genre))]
        public string? Genre { get; set; }
        [Column("price")]
        public double Price { get; set; }
        [Column("publisher")]
        [MaxLength(50)]
        public string? Publisher { get; set; }
        [Column("number_of_pages")]
        public int? NumberOfPages { get; set; }
        [Column("book_format")]
        [MaxLength(15)]
        [EnumDataType(typeof(BookFormat))]
        public string BookFormat { get; set; }
        [Column("publication_date")]
        public DateTime PublicationDate { get; set; }
        [Column("book_language")]
        [MaxLength(20)]
        [EnumDataType(typeof(BookLanguage))]
        public string BookLanguage { get; set; }

        //Relationships
        public List<Book_Author> BooksAuthors { get; set; }
    }
}
