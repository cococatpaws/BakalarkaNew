using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using webapi.Data;
using webapi.Models;
using webapi.Service;

namespace webapi.Controllers
{
    public class BookPageController : ControllerBase
    {
        private readonly ISqlService SqlService;

        public BookPageController(ISqlService varSqlService)
        {
            this.SqlService = varSqlService;
        }

        [HttpGet("knihy")]
        public async Task<IActionResult> GetAllBooksWithAuthors()
        {
            var allBooks = await SqlService.GetAllBooksWithAuthors();
            /*
             * Na toto treba doinstalovat dotnet add package Newtonsoft.Json
             * JObject responseObject = JObject.Parse(allBooks);

            // Get the inner array without the outer properties
            JArray booksArray = (JArray)responseObject["$values"];

            // Convert the array to a list of Book objects
            List<Book> allBooksEdited = booksArray.ToObject<List<Book>>();*/
            return Ok(allBooks);
        }

    }
}
