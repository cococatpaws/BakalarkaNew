using webapi.Data;
using webapi.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using webapi.Migrations;

namespace webapi.Service
{
    public class SqlService : ISqlService
    {
        private readonly DataContext dbContext;

        public SqlService(DataContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ActionResult<List<Book>>> GetAllBooksWithAuthors()
        {
            return await dbContext.Books.ToListAsync();
            /*   return await dbContext.Books.Include(b => b.BooksAuthors).ThenInclude(ba => ba.Author) // Přidáme zahrnutí autorů
           .ToListAsync();*/
        }
    }
    
}
