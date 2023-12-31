﻿using webapi.Data;
using webapi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using webapi.Models.ResponseModels;
using webapi.Models.UtilityModels;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using webapi.Enums;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Win32;

namespace webapi.Service
{
    public class SqlService : ISqlService
    {
        private readonly DataContext dbContext;
        private readonly IJwtService jwtService;
        private readonly IAzureBlobService AzureBlobService;

        public SqlService(DataContext dbContext, IJwtService jwtService, IAzureBlobService azureBlobService)
        {
            this.dbContext = dbContext;
            this.jwtService = jwtService;
            this.AzureBlobService = azureBlobService;
        }

        public async Task<ActionResult<List<Book>>> GetAllBooks()
        {
            return await dbContext.Books.ToListAsync();
        }
        public async Task<ActionResult<List<BookResponse>>> GetAllBooksWithAuthors()
        {
            var books = await dbContext.Books.Include(b => b.BooksAuthors).ThenInclude(ba => ba.Author) // Přidáme zahrnutí autorů
           .ToListAsync();

            var booksWithoutOrders = books.Select(b => new BookResponse
            {
               BookId= b.BookId,
               Title= b.Title,
               Price= b.Price,

            }).ToList();

            return booksWithoutOrders;
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

        public async Task<bool> SaveBookCover(BookCoverResponse model)
        {
            if (model.CoverImage != null)
            {
                var book = await dbContext.Books.FindAsync(model.BookId);

                if (book != null)
                {
                    var coverImageUrlBook = "";
                    var picture = model.CoverImage;

                    if (model.CoverImage != null)
                    {
                        coverImageUrlBook = await this.AzureBlobService.UploadImageAsync(model.CoverImage);

                        if (coverImageUrlBook != null)
                        {
                            book.CoverImageURL = coverImageUrlBook;
                            await dbContext.SaveChangesAsync();
                            return true;
                        } else
                        {
                            return false;
                        }
                    } else
                    {
                        return false;
                    }
                } else
                {
                    return false;
                }
            } else
            {
                return false;
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

                //Vymazanie image z blobstorage
                var bookCoverURL = bookToDelete.CoverImageURL;
                await this.AzureBlobService.RemoveImageAsync(bookCoverURL);

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

                //AK DeleteImage == TRUE -> chcem vymazať starú knihu z blob service a URL z DB
                if (book.DeleteImage != null && book.DeleteImage == true)
                {
                    var bookId = book.CoverImageURL;
                    await this.AzureBlobService.RemoveImageAsync(bookId.ToString());

                    existingBook.CoverImageURL = "";
                    await dbContext.SaveChangesAsync();
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


        public async Task<string> Login(Login userInfo)
        {
                try
                {

                    var varUser = await dbContext.Users.FirstOrDefaultAsync(u => u.UserName == userInfo.UserName);
                  
                    if (varUser == null)
                    {
                    throw new CustomException(StatusCodes.Status404NotFound, $"Takyto pouzivatel neexistuje");
                    }

                    bool userExists = BCrypt.Net.BCrypt.EnhancedVerify(userInfo.Password, varUser.Password);

                    if (userExists)
                    {
                    var token = jwtService.CreateJwt(varUser);
                    return token;
                    } else
                    {
                        throw new CustomException(StatusCodes.Status401Unauthorized, $"Neplatne meno alebo heslo");
                    }
            } catch (Exception ex)
                {
                    throw new CustomException(StatusCodes.Status500InternalServerError, $"Error while trying to log in: {ex}");
                }
        }

        public async Task<bool> Register(Register register)
        {

            try
            {
                if (register != null && register.UserName != null && register.Password != null && register.Name != null && register.Surname != null && register.Email != null && register.PhoneNumber != null
                    && register.Country != null && register.City != null && register.Street != null && register.AddressNumber != null && register.PostCode != null)
                {


                    if (await this.CheckExistence(register.UserName, "username"))
                    {
                        throw new CustomException(StatusCodes.Status409Conflict, $"Pouzivatel s menom {register.UserName} uz existuje");
                    } else if (await this.CheckExistence(register.Email, "email"))
                    {
                        throw new CustomException(StatusCodes.Status409Conflict, $"Pouzivatel s emailom {register.Email} uz existuje");
                    } else if (await this.CheckExistence(register.PhoneNumber, "phonenumber"))
                    {
                        throw new CustomException(StatusCodes.Status409Conflict, $"Pouzivatel s telefonnym cislom {register.PhoneNumber} uz existuje");
                    }

                    //PersonalInfo
                    var personalInfo = new PersonalInfo
                    {
                        Name = register.Name,
                        Surname = register.Surname,
                        Email = register.Email,
                        PhoneNumber = register.PhoneNumber
                    };

                    await dbContext.PersonalInfo.AddAsync(personalInfo);

                    var userType = new UserType
                    {
                    };

                    await dbContext.UserTypes.AddAsync(userType);
                    await dbContext.SaveChangesAsync();

                    var userTypeDB = await dbContext.UserTypes.OrderBy(ut => ut.UserTypeId).LastOrDefaultAsync();


                    //Address
                    var addressSh = new Address
                    {
                        Country = register.Country,
                        City = register.City,
                        Street = register.Street,
                        AddressNumber = register.AddressNumber,
                        PostCode = register.PostCode
                    };

                    await dbContext.Addresses.AddAsync(addressSh);
                    await dbContext.SaveChangesAsync();
                    var addressShipping = await dbContext.Addresses.OrderBy(ad => ad.AddressId).LastOrDefaultAsync();

                    //Shipping address
                    var shippingAddress = new ShippingAddress
                    {
                        AddressIdS = addressShipping!.AddressId,
                        ShippingDetails = "",
                        UserTypeId = userTypeDB!.UserTypeId
                    };

                    await dbContext.ShippingAddresses.AddAsync(shippingAddress);
                    await dbContext.SaveChangesAsync();

                    //Billing Address
                    var addressBi = new Address
                    {
                        Country = register.Country,
                        City = register.City,
                        Street = register.Street,
                        AddressNumber = register.AddressNumber,
                        PostCode = register.PostCode
                    };

                    await dbContext.Addresses.AddAsync(addressBi);
                    await dbContext.SaveChangesAsync();
                    var addressBilling = await dbContext.Addresses.OrderBy(ad => ad.AddressId).LastOrDefaultAsync();

                    var billingAddress = new BillingAddress
                    {
                        AddressIdB = addressBilling!.AddressId,
                        UserTypeId = userTypeDB!.UserTypeId
                    };

                    await dbContext.BillingAddresses.AddAsync(billingAddress);
                    await dbContext.SaveChangesAsync();

                    //UserType
                    var billingAddressDB = await dbContext.BillingAddresses.OrderBy(ba => ba.AddressIdB).LastOrDefaultAsync();
                    var shippingAddressDB = await dbContext.ShippingAddresses.OrderBy(sa => sa.AddressIdS).LastOrDefaultAsync();
                    
                    var personalInfoDB = await dbContext.PersonalInfo.OrderBy(pi => pi.PersonalInfoId).LastOrDefaultAsync();
                    var password = register.Password;
                    var passwordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(password, 13);

                    var user = new User
                    {
                        UserTypeIdUser = userTypeDB.UserTypeId,
                        PersonalInfoIdUser = personalInfo.PersonalInfoId,
                        ProfilePictureUrl = "",
                        UserName = register.UserName,
                        Password = passwordHash,
                        Role = "User"
                    };

                    await dbContext.Users.AddAsync(user);
                    await dbContext.SaveChangesAsync();

                    return true;

                } else
                {
                    throw new CustomException(StatusCodes.Status400BadRequest, $"Niektore polia neboli vyplnene!");
                }

            } catch (Exception ex)
            {
                throw new CustomException(StatusCodes.Status500InternalServerError, $"Nieco sa dogabalo: {ex}");
            }
        }

        public async Task<bool> CheckExistence(string variableToCheck, string typeOfObject)
        {
            if (typeOfObject == "username")
            {
                var usernameExisting = await dbContext.Users.FirstOrDefaultAsync(u => u.UserName == variableToCheck);
                if (usernameExisting != null)
                {
                    return true;

                }
            } else if (typeOfObject == "email")
            {
                var emailExisting = await dbContext.PersonalInfo.FirstOrDefaultAsync(e => e.Email == variableToCheck);
                if(emailExisting != null) 
                { 
                    return true; 
                }
            } else if (typeOfObject == "phonenumber")
            {
                var phoneNumberExisting = await dbContext.PersonalInfo.FirstOrDefaultAsync(e => e.PhoneNumber == variableToCheck);
                if (phoneNumberExisting != null)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> Order(OrderResponse order)
        {
            try
            {
                if (order.Username != null)
                {
                    webapi.Models.BillingAddress billingAddressDB = new BillingAddress { };
                    webapi.Models.ShippingAddress shippingAddressDB = new ShippingAddress { };

                    var existingUser = await dbContext.Users.FirstOrDefaultAsync(u => u.UserName == order.Username);
                    var existingShippingAddress = await dbContext.ShippingAddresses.OrderBy(sa => sa.ShippingAddressId).LastOrDefaultAsync(sa => sa.UserTypeId == existingUser!.UserTypeIdUser);
                    if (existingShippingAddress != null)
                    {
                        var existingAddress = await dbContext.Addresses.FirstOrDefaultAsync(a => a.AddressId == existingShippingAddress.AddressIdS);
                        if (existingAddress.AddressNumber == order.AddressNumber && existingAddress.Country == order.Country && existingAddress.City == order.City
                            && existingAddress.Street == order.Street)
                        {
                            billingAddressDB = await dbContext.BillingAddresses.OrderBy(ba => ba.BillingAddressId).LastOrDefaultAsync(ba => ba.UserTypeId == existingUser.UserTypeIdUser);
                            shippingAddressDB = await dbContext.ShippingAddresses.OrderBy(sa => sa.ShippingAddressId).LastOrDefaultAsync(sa => sa.UserTypeId == existingUser.UserTypeIdUser);
                            
                        } else
                        {
                            var addressSh = new Address
                            {
                                Country = order.Country,
                                City = order.City,
                                Street = order.Street,
                                AddressNumber = order.AddressNumber,
                                PostCode = order.PostCode
                            };

                            await dbContext.Addresses.AddAsync(addressSh);
                            await dbContext.SaveChangesAsync();
                            var addressShipping = await dbContext.Addresses.OrderBy(ad => ad.AddressId).LastOrDefaultAsync();

                            //Shipping address

                            var shippingAddress = new ShippingAddress
                            {
                                AddressIdS = addressShipping!.AddressId,
                                ShippingDetails = "",
                                UserTypeId = existingUser.UserTypeIdUser
                            };

                            await dbContext.ShippingAddresses.AddAsync(shippingAddress);
                            await dbContext.SaveChangesAsync();

                            //Billing Address
                            var addressBi = new Address
                            {
                                Country = order.Country,
                                City = order.City,
                                Street = order.Street,
                                AddressNumber = order.AddressNumber,
                                PostCode = order.PostCode
                            };

                            await dbContext.Addresses.AddAsync(addressBi);
                            await dbContext.SaveChangesAsync();
                            var addressBilling = await dbContext.Addresses.OrderBy(ad => ad.AddressId).LastOrDefaultAsync();

                            var billingAddress = new BillingAddress
                            {
                                AddressIdB = addressBilling!.AddressId,
                                UserTypeId = (int)existingUser.UserTypeIdUser
                            };

                            await dbContext.BillingAddresses.AddAsync(billingAddress);
                            await dbContext.SaveChangesAsync();

                            //UserType
                            billingAddressDB = await dbContext.BillingAddresses.OrderBy(ba => ba.AddressIdB).LastOrDefaultAsync();
                            shippingAddressDB = await dbContext.ShippingAddresses.OrderBy(sa => sa.AddressIdS).LastOrDefaultAsync();
                        }
                    }

                        var orderVar = new Order
                        {
                            PaymentTypeId = order.PaymentTypeId,
                            ShippingTypeId = order.ShippingTypeId,
                            UserTypeId = existingUser.UserTypeIdUser,
                            DatePlaced = DateTime.Now,
                            OrderStatus = "OrderCreated",
                            OrderType = order.OrderType,
                            OrderDetails = order.OrderDetails,
                            ShippingAddressIdO = shippingAddressDB!.ShippingAddressId,
                            BillingAddressIdO = billingAddressDB.BillingAddressId,
                            ShippingAddress = shippingAddressDB,
                            BillingAddress = billingAddressDB,
                        };

                        await dbContext.Order.AddAsync(orderVar);
                        await dbContext.SaveChangesAsync();
                        var orderDb = await dbContext.Order.OrderBy(o => o.OrderID).LastOrDefaultAsync();

                        var varOrderBooks = order.BooksInOrder.Select((bookInOrder, index) =>
                        {
                            var existingBook = dbContext.Books.FirstOrDefault(b => b.BookId == bookInOrder.BookId);

                            if (existingBook != null)
                            {
                                var varOrderBook = new Order_Book
                                {
                                    OrderId = orderDb.OrderID,
                                    Order = orderDb,
                                    BookId = existingBook.BookId,
                                    Book = existingBook,
                                    BookPrice = bookInOrder.BookPrice,
                                    QuantityOrdered = bookInOrder.QuantityOrdered
                                };
                                return varOrderBook;
                            }

                            return null;
                        }).Where(orderBook => orderBook != null).ToList();

                        dbContext.Orders_Books.AddRange(varOrderBooks);
                        await dbContext.SaveChangesAsync();
                    return true;
                } else
                {
                    var userType = new UserType { };
                    await dbContext.UserTypes.AddAsync(userType);
                    await dbContext.SaveChangesAsync();

                    var userTypeDB = await dbContext.UserTypes.OrderBy(ut => ut.UserTypeId).LastOrDefaultAsync();

                    var personalInfo = new PersonalInfo
                    {
                        Name = order.Name,
                        Surname = order.Surname,
                        Email = order.Email,
                        PhoneNumber = order.PhoneNumber
                    };

                    await dbContext.PersonalInfo.AddAsync(personalInfo);
                    await dbContext.SaveChangesAsync();

                    var personalInfoDB = await dbContext.PersonalInfo.OrderBy(pi => pi.PersonalInfoId).LastOrDefaultAsync();


                    var temporatyUser = new TemporaryUser
                    {
                        UserTypeIdTempUser = userTypeDB!.UserTypeId,
                        PersonalInfoIdTempUser = personalInfoDB!.PersonalInfoId
                    };

                    await dbContext.TemporaryUsers.AddAsync(temporatyUser);
                    await dbContext.SaveChangesAsync();

                    var addressSh = new Address
                    {
                        Country = order.Country,
                        City = order.City,
                        Street = order.Street,
                        AddressNumber = order.AddressNumber,
                        PostCode = order.PostCode
                    };

                    await dbContext.Addresses.AddAsync(addressSh);
                    await dbContext.SaveChangesAsync();
                    var addressShipping = await dbContext.Addresses.OrderBy(ad => ad.AddressId).LastOrDefaultAsync();

                    //Shipping address
                    var shippingAddress = new ShippingAddress
                    {
                        AddressIdS = addressShipping!.AddressId,
                        ShippingDetails = "",
                        UserTypeId = userTypeDB!.UserTypeId
                    };

                    await dbContext.ShippingAddresses.AddAsync(shippingAddress);
                    await dbContext.SaveChangesAsync();

                    //Billing Address
                    var addressBi = new Address
                    {
                        Country = order.Country,
                        City = order.City,
                        Street = order.Street,
                        AddressNumber = order.AddressNumber,
                        PostCode = order.PostCode
                    };

                    await dbContext.Addresses.AddAsync(addressBi);
                    await dbContext.SaveChangesAsync();
                    var addressBilling = await dbContext.Addresses.OrderBy(ad => ad.AddressId).LastOrDefaultAsync();

                    var billingAddress = new BillingAddress
                    {
                        AddressIdB = addressBilling!.AddressId,
                        UserTypeId = userTypeDB!.UserTypeId
                    };

                    await dbContext.BillingAddresses.AddAsync(billingAddress);
                    await dbContext.SaveChangesAsync();

                    //UserType
                    var billingAddressDB = await dbContext.BillingAddresses.OrderBy(ba => ba.AddressIdB).LastOrDefaultAsync();
                    var shippingAddressDB = await dbContext.ShippingAddresses.OrderBy(sa => sa.AddressIdS).LastOrDefaultAsync();

                    var orderVar = new Order
                    {
                        PaymentTypeId = order.PaymentTypeId,
                        ShippingTypeId = order.ShippingTypeId,
                        UserTypeId = userTypeDB.UserTypeId,
                        DatePlaced = DateTime.Now,
                        OrderStatus = "OrderCreated",
                        OrderType = order.OrderType,
                        OrderDetails = order.OrderDetails,
                        ShippingAddressIdO = shippingAddressDB.ShippingAddressId,
                        BillingAddressIdO = billingAddressDB.BillingAddressId,
                        ShippingAddress = shippingAddressDB,
                        BillingAddress = billingAddressDB,
                    };

                    await dbContext.Order.AddAsync(orderVar);
                    await dbContext.SaveChangesAsync();
                    var orderDb = await dbContext.Order.OrderBy(o => o.OrderID).LastOrDefaultAsync();

                    var varOrderBooks = order.BooksInOrder.Select((bookInOrder, index) =>
                    {
                        var existingBook = dbContext.Books.FirstOrDefault(b => b.BookId == bookInOrder.BookId);

                        if (existingBook != null)
                        {
                            var varOrderBook = new Order_Book
                            {
                                OrderId = orderDb.OrderID,
                                Order = orderDb,
                                BookId = existingBook.BookId,
                                Book = existingBook,
                                BookPrice = bookInOrder.BookPrice,
                                QuantityOrdered = bookInOrder.QuantityOrdered
                            };
                            return varOrderBook;
                        }

                        return null;
                    }).Where(orderBook => orderBook != null).ToList();

                    dbContext.Orders_Books.AddRange(varOrderBooks);
                    await dbContext.SaveChangesAsync();

                    return true;
                }
            } catch
            {
                throw new CustomException(StatusCodes.Status500InternalServerError, "Error");
            }
        }

    }
    
}
