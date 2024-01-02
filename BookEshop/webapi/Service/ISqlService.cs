using Microsoft.AspNetCore.Mvc;
using webapi.Models;
using webapi.Models.ResponseModels;

namespace webapi.Service
{
    public interface ISqlService
    {
        public Task<ActionResult<List<Book>>> GetAllBooks();
        public Task<ActionResult<List<BookResponse>>> GetAllBooksWithAuthors();
        public Task<ActionResult<Book>> GetBookByID(int bookId);
        public Task<ActionResult<Book>> AddBook(BookResponse model);
        public Task<bool> SaveBookCover(BookCoverResponse model);
        public Task<bool> DeleteBook(int bookId);
        public Task<bool> EditBook(BookResponse book);
        public Task<string> Login(Login userInfo);
        public Task<bool> Register(Register register);
        public Task<bool> CheckExistence(string variableToCheck, string typeOfObject);

        public Task<bool> Order(OrderResponse order);

        //public Task<ActionResult<List<Book>>> AddBook(BookResponse model);
        //public Task<List<Book>> GetAllBooksWithAuthors();
    }
}
