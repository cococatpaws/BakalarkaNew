using Microsoft.EntityFrameworkCore;
using webapi.Models;

namespace webapi.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) 
        {
            //InitializeDatabase();
        }

        //Toto tu musim mat, aby sa mi to zobrazilo v databaze ako tabulka
        public DbSet<Book> Books => Set<Book>();
        public DbSet<Author> Authors => Set<Author>();
        public DbSet<Book_Author> Books_Authors => Set<Book_Author>();

        
        /*private void InitializeDatabase()
        {
            OnModelCreating(new ModelBuilder());
        }*/

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book_Author>().HasKey(ba => new
            {
                ba.BookId,
                ba.AuthorId
            });

            modelBuilder.Entity<Book_Author>().HasOne(ba => ba.Book).WithMany(b => b.BooksAuthors).HasForeignKey(ba => ba.BookId);
            modelBuilder.Entity<Book_Author>().HasOne(ba => ba.Author).WithMany(a => a.Books_Authors).HasForeignKey(ba => ba.AuthorId);
        }
    }
}
