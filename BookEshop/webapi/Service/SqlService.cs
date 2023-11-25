using webapi.Data;
using webapi.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using webapi.Migrations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using webapi.Enums;
using Microsoft.AspNetCore.Http.HttpResults;

namespace webapi.Service
{
    public class SqlService : ISqlService
    {
        private readonly DataContext dbContext;

        public SqlService(DataContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ActionResult<List<Book>>> GetAllBooks()
        {
            return await dbContext.Books.ToListAsync();
        }
        public async Task<ActionResult<List<Book>>> GetAllBooksWithAuthors()
        {
            return await dbContext.Books.Include(b => b.BooksAuthors).ThenInclude(ba => ba.Author) // Přidáme zahrnutí autorů
           .ToListAsync();
        }

        public async Task<ActionResult<Book>> AddBook(BookResponse model)
        {
            try
            {
                //Najprv skontrolujem ci je kniha uz ulozena v DB
                var book = await dbContext.Books.FirstOrDefaultAsync(b => b.Title == model.Title && b.PublicationDate == model.PublicationDate
                && b.BookFormat == model.BookFormat);
                if (book != null)
                {
                    return new Book { BookId = -1};
                }

                // Pre každého autora v poli booksAuthors
                foreach (var author in model.BooksAuthors)
                {
                    // Skontrolujte, či autor už existuje v databáze
                        var existingAuthor = await dbContext.Authors
                        .FirstOrDefaultAsync(a =>
                            a.Name == author.Name &&
                            a.MiddleName == author.MiddleName &&
                            a.Surname == author.Surname);

                    // Ak autor neexistuje, pridajte ho do databázy
                    if (existingAuthor == null)
                    {
                        var newAuthor = new Author
                        {
                            Name = author.Name,
                            MiddleName = author.MiddleName,
                            Surname = author.Surname
                        };

                        dbContext.Authors.Add(newAuthor);
                        await dbContext.SaveChangesAsync();
                    }
                }

                // Pridajte knihu do databázy
                var newBook = new Book
                {
                    Title = model.Title,
                    Description = model.Description,
                    QuantityInStock = model.QuantityInStock,
                    CoverImageURL = model.CoverImageURL,
                    Genre = model.Genre,
                    Price = model.Price,
                    Publisher = model.Publisher,
                    NumberOfPages = model.NumberOfPages,
                    BookFormat = model.BookFormat,
                    PublicationDate = model.PublicationDate,
                    BookLanguage = model.BookLanguage,
                };

                dbContext.Books.Add(newBook);
                await dbContext.SaveChangesAsync();

                var id = newBook.BookId;

                //Pridanie vztahu
                var varBookAuthors = model.BooksAuthors.Select((author, index) =>
                {
                    var existingAuthor = dbContext.Authors.FirstOrDefault(a =>
                        a.Name == author.Name &&
                        a.MiddleName == author.MiddleName &&
                        a.Surname == author.Surname);

                    if (existingAuthor != null)
                    {
                        var varBookAuthor = new Book_Author
                        {
                            BookId = newBook.BookId,
                            Book = newBook,
                            AuthorId = existingAuthor.AuthorId,
                            Author = existingAuthor,
                            AuthorOrder = index
                        };
                        return varBookAuthor;
                    }

                    return null;
                }).Where(author => author != null).ToList();

                // Pridajte asociatívne entity do kontextu databázy
                dbContext.AddRange(varBookAuthors);
                await dbContext.SaveChangesAsync();

                return await dbContext.Books.FirstOrDefaultAsync(b => b.Title == model.Title && b.PublicationDate == model.PublicationDate && b.BookFormat == model.BookFormat
                && b.Publisher == model.Publisher && b.NumberOfPages == model.NumberOfPages &&b.QuantityInStock == model.QuantityInStock);
            }
            catch (Exception ex)
            {
                throw new CustomException(StatusCodes.Status500InternalServerError, "Error occured while trying to save book to DB.");
            }
        }

        public async Task<bool> DeleteBook(int bookId)
        {
            try
            {
                //Najprv vymazat z Books_Authors vztahy a potom knihu, autorov mozem nechat, lebo mozu byt vo viacerych knihach
                var bookToDelete = dbContext.Books.Find(bookId);
                if (bookToDelete == null )
                {
                    throw new CustomException(StatusCodes.Status404NotFound, $"Book with this id: {bookId} is not in DB.");
                }

                var recordsToDelete = dbContext.Books_Authors.Where(ba => ba.BookId == bookId);
                if (recordsToDelete.Any())
                {
                    dbContext.Books_Authors.RemoveRange(recordsToDelete);
                    await dbContext.SaveChangesAsync();
                }

                dbContext.Books.Remove(bookToDelete);
                await dbContext.SaveChangesAsync();

                var deletedBook = dbContext.Books.Find(bookId);
                var deletedRecords = dbContext.Books_Authors.Where(ba => ba.BookId == bookId);

                if (deletedBook == null && !deletedRecords.Any())
                {
                    return true;
                } else
                {
                    return false;
                }
            } catch (Exception ex)
            {
                throw new CustomException(StatusCodes.Status500InternalServerError, $"Error occured while trying to delete book with ID: {bookId} from DB.");
            }
        }

    }
    
}
