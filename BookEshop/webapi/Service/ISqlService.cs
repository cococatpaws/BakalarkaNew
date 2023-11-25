using Microsoft.AspNetCore.Mvc;
using webapi.Models;

namespace webapi.Service
{
    public interface ISqlService
    {
        public Task<ActionResult<List<Book>>> GetAllBooks();
        public Task<ActionResult<List<Book>>> GetAllBooksWithAuthors();
        public Task<ActionResult<Book>> AddBook(BookResponse model);
        public Task<bool> DeleteBook(int bookId);
        //public Task<ActionResult<List<Book>>> AddBook(BookResponse model);
        //public Task<List<Book>> GetAllBooksWithAuthors();
    }
}
