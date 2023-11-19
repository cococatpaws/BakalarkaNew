using Microsoft.AspNetCore.Mvc;
using webapi.Models;

namespace webapi.Service
{
    public interface ISqlService
    {
        public Task<ActionResult<List<Book>>> GetAllBooksWithAuthors();
        //public Task<List<Book>> GetAllBooksWithAuthors();
    }
}
