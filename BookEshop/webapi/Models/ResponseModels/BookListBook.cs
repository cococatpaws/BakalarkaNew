using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using webapi.Enums;

namespace webapi.Models.ResponseModels
{
    public class BookListBook
    {
        [Column("id_book")]
        public int BookId { get; set; }
        [Column("title")]
        [MaxLength(60)]
        public string Title { get; set; }
        [Column("cover_image_url")]
        [MaxLength(255)]
        public string CoverImageURL { get; set; }
        [Column("price")]
        public double Price { get; set; }

        public List<Author> Authors { get; set; }
    }
}
