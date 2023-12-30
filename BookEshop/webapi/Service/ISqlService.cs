using Microsoft.AspNetCore.Mvc;
using webapi.Models;
using webapi.Models.ResponseModels;

namespace webapi.Service
{
    public interface ISqlService
    {
        public Task<ActionResult<List<Book>>> GetAllBooks();
        public Task<ActionResult<List<Book>>> GetAllBooksWithAuthors();
        public Task<ActionResult<Book>> GetBookByID(int bookId);
        public Task<ActionResult<Book>> AddBook(BookResponse model);
        public Task<bool> DeleteBook(int bookId);
        public Task<bool> EditBook(BookResponse book);
        public Task<ActionResult<User>> Login(Login userInfo);
        public Task<bool> Register(Register register);
        public Task<bool> CheckExistence(string variableToCheck, string typeOfObject);

        //public Task<ActionResult<List<Book>>> AddBook(BookResponse model);
        //public Task<List<Book>> GetAllBooksWithAuthors();
    }
}
