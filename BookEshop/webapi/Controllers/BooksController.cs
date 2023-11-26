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

        [HttpGet("knihySAutormi")]
        public async Task<IActionResult> GetAllBooksWithAuthors()
        {
            var allBooks = await SqlService.GetAllBooksWithAuthors();
            return Ok(allBooks);
        }

        [HttpGet("knihy")]
        public async Task<IActionResult> GetAllBooks()
        {
            var allBooks = await SqlService.GetAllBooks();
            return Ok(allBooks);
        }

        [HttpGet("edit-book/{bookId}")]
        public async Task<IActionResult> GetBookByID(int bookId)
        {
            try
            {
                var result = await SqlService.GetBookByID(bookId);
                return Ok(result);
            } catch (Exception ex)
            {
                throw new CustomException(StatusCodes.Status500InternalServerError, $"Error occured while trying to save book to DB: {ex}");
            }
        }

        [HttpPost("saveBook")]
        public async Task<IActionResult> SaveBook([FromBody] BookResponse model)
        {
            try
            {
                Console.WriteLine($"Received book data from frontend: {model}");

                var result = await SqlService.AddBook(model);

                // Ak výsledok nie je null a obsahuje informácie o knihe, odpovedzte s týmito informáciami
                if (result.Value != null && result.Value.BookId != -1)
                {
                    return Ok(result);
                }
                else if (result.Value != null && result.Value.BookId == -1)
                {
                    throw new CustomException(StatusCodes.Status409Conflict, "Book was already saved in DB.");
                } else
                {
                    throw new CustomException(StatusCodes.Status500InternalServerError, "Error occured while trying to save book to DB.");
                }
            }
            catch (Exception ex)
            {
                // Ak sa vyskytne chyba, môžete ju zachytiť a odpovedať s chybovou správou
                return BadRequest(new { message = $"Chyba pri spracovaní údajov: {ex.Message}" });
            }
        }

        [HttpDelete("/knihy")]
        public async Task<IActionResult> DeleteBook([FromBody] int bookId)
        {
            try
            {
                var result = await SqlService.DeleteBook(bookId);
                return Ok(result);
            } catch (Exception ex)
            {
                throw new CustomException(StatusCodes.Status500InternalServerError, $"Error occured while trying to save book to DB: {ex}");
            }
        }

        [HttpPut("edit-book/{bookId}")]
        public async Task<IActionResult> EditBook([FromBody] BookResponse book)
        {
            try
            {
                var result = await SqlService.EditBook(book);
                return Ok(result);
            } catch (Exception ex)
            {
                throw new CustomException(StatusCodes.Status500InternalServerError, $"Error occured while trying to save book to DB: {ex}");
            }
        }
    }
}
