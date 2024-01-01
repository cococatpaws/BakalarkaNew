namespace webapi.Models.ResponseModels
{
    public class BookCoverResponse
    {
        public int BookId { get; set; }
        public IFormFile? CoverImage { get; set; }
    }
}
