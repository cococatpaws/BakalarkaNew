using System.ComponentModel.DataAnnotations.Schema;

namespace webapi.Models
{
    [Table("books_authors")]
    public class Book_Author
    {
        public int BookId { get; set; }
        public Book Book { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; }
        [Column("author_order")]
        public int AuthorOrder { get; set; }


    }
}
