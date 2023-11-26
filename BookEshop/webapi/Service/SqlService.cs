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
using System.Net;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        public async Task<ActionResult<Book>> GetBookByID(int bookId)
        {
            try
            {
                var book = await dbContext.Books.Where(b => b.BookId == bookId).Include(b => b.BooksAuthors).ThenInclude(ba => ba.Author)
                                .FirstOrDefaultAsync();

                if (book == null)
                {
                    throw new CustomException(StatusCodes.Status404NotFound, $"Book with this id {bookId} wasn't found in the DB.");
                }

                return book;
            } catch (Exception ex)
            {
                throw new CustomException(StatusCodes.Status500InternalServerError, $"Error occured while trying to delete book with ID: {bookId} from DB.");
            }
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
                            AuthorOrder = index + 1
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
                var bookToDelete = await dbContext.Books.FindAsync(bookId);
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

        public async Task<bool> EditBook(BookResponse book)
        {
            try
            {
                var existingBook = await dbContext.Books.FindAsync(book.BookId);

                if (existingBook == null )
                {
                    throw new CustomException(StatusCodes.Status404NotFound, $"Book with ID {book.BookId} not found.");
                }

                //Najprv skontrolovat, ci su vsetci autori v tabulke Authors
                foreach (var author in book.BooksAuthors)
                {
                    var existingAuthor = await dbContext.Authors
                    .FirstOrDefaultAsync(a =>
                        a.Name == author.Name &&
                        a.MiddleName == author.MiddleName &&
                        a.Surname == author.Surname);

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

                //Potom skontrolovat, ci su v tabulke BookAuthors poprepajani autori tak ako maju byt -> ci nechyba nejaky vztah
                //alebo nie je nejaky navyse
                // Porovnajte existujúce vzťahy so zoznamom autorov
                // Načítajte existujúce vztahy z asociatívnej entity pre daný BookId
                var existingBookAuthors = await dbContext.Books_Authors
                    .Where(ba => ba.BookId == book.BookId)
                    .ToListAsync();

                //Najprv vymazem vsetky vztahy (mazem vsetky, aby som mala jednoduchsie spravit order autorov - maju order podla toho, v akom poradi sa zadaju do inputu)
                //dbContext.Books_Authors.RemoveRange(existingBookAuthors);
                for (int i = 0; i < existingBookAuthors.Count; i++)
                {
                    var existingAuthor = existingBookAuthors[i];
                    var found = false;
                    for (int j = 0; j < book.BooksAuthors?.Count; j++)
                    {
                        var author = await dbContext.Authors.FirstOrDefaultAsync(a => a.Name == book.BooksAuthors[j].Name && a.MiddleName == book.BooksAuthors[j].MiddleName
                                                                                    && a.Surname == book.BooksAuthors[j].Surname);
                        if (existingBookAuthors[i].AuthorId == author?.AuthorId)
                        {
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        var relationshipToRemove = await dbContext.Books_Authors.FirstOrDefaultAsync(ba => ba.AuthorId == existingBookAuthors[i].AuthorId && ba.BookId == book.BookId);
                        if (relationshipToRemove != null)
                            dbContext.Books_Authors.Remove(relationshipToRemove);
                    }
                }

                //Teraz idem pridat vsetky vztahy nanovo
                /*for (int i = 0; i < book.BooksAuthors?.Count; i++)
                {
                    var author = await dbContext.Authors.FirstOrDefaultAsync(a => a.Name == book.BooksAuthors[i].Name && a.MiddleName == book.BooksAuthors[i].MiddleName
                                                                                        && a.Surname == book.BooksAuthors[i].Surname);
                    if (author != null)
                    {
                        var newRelationship = new Book_Author
                        {
                            AuthorId = author.AuthorId,
                            BookId = book.BookId,
                            AuthorOrder = i + 1
                        };

                        dbContext.Books_Authors.Add(newRelationship);
                    }
                }*/

                    //Tu bola kontrola, ci sa uz vztah tam nachadza a ak nie, tak sa pridal
                    for (int i = 0; i < book.BooksAuthors?.Count; i++)
                    {
                        var author = await dbContext.Authors.FirstOrDefaultAsync(a => a.Name == book.BooksAuthors[i].Name && a.MiddleName == book.BooksAuthors[i].MiddleName
                                                                                        && a.Surname == book.BooksAuthors[i].Surname);
                        var relationship = await dbContext.Books_Authors.FirstOrDefaultAsync(ba => ba.AuthorId == author.AuthorId &&  ba.BookId == book.BookId);
                        if (relationship == null)
                        {
                            var newRelationship = new Book_Author
                            {
                                AuthorId = author.AuthorId,
                                BookId = book.BookId,
                            };

                            dbContext.Books_Authors.Add(newRelationship);
                        }
                    }

                    var relationships = await dbContext.Books_Authors
                    .Where(ba => ba.BookId == book.BookId)
                    .ToListAsync();

                for (int i = 0; i < relationships.Count; i++)
                {
                    relationships[i].AuthorOrder = i + 1;
                }

                existingBook.Title = book.Title; 
                existingBook.Description = book.Description;
                existingBook.QuantityInStock = book.QuantityInStock;
                existingBook.Genre = book.Genre;
                existingBook.Price = book.Price;
                existingBook.Publisher = book.Publisher;
                existingBook.NumberOfPages = book.NumberOfPages;
                existingBook.BookFormat = book.BookFormat;
                existingBook.PublicationDate = book.PublicationDate;
                existingBook.BookLanguage = book.BookLanguage;

                await dbContext.SaveChangesAsync();

                return true;

            } catch (Exception ex)
            {
                throw new CustomException(StatusCodes.Status500InternalServerError, $"Error occured while trying to edit book with ID: {book.BookId} from DB.");
            }
        }

    }
    
}
