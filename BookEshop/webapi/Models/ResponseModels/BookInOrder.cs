namespace webapi.Models.ResponseModels
{
    public class BookInOrder
    {
        public int BookId { get; set; }
        public int QuantityOrdered { get; set; }
        public double BookPrice { get; set; }
    }
}
