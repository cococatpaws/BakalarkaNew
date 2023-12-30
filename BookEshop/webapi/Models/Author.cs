using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webapi.Models
{
    [Table("author")]
    public class Author
    {
        [Key]
        [Column("id_author")]
        public int AuthorId { get; set; }
        [Column("name")]
        [MaxLength(50)]
        public string Name { get; set; }
        [Column("middle_name")]
        [MaxLength(50)]
        public string? MiddleName { get; set; }
        [Column("surname")]
        [MaxLength(50)]
        public string? Surname { get; set; }

        //Relationship
        public List<Book_Author> Books_Authors { get; set; }
    }
}
