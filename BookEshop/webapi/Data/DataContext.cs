using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
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
        public DbSet<User> Users => Set<User>();
        public DbSet<TemporaryUser> TemporaryUsers => Set<TemporaryUser>();
        public DbSet<UserType> UserTypes => Set<UserType>();
        public DbSet<Address> Addresses => Set<Address>();
        public DbSet<ShippingAddress> ShippingAddresses => Set<ShippingAddress>();
        public DbSet<BillingAddress> BillingAddresses => Set<BillingAddress>();
        public DbSet<PersonalInfo> PersonalInfo => Set<PersonalInfo>(); 

        
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


            //1 to 1 relationships
            //isRequired znamena, ze kazdy zaznam v tabulke user musi mat odpovedajuci zaznam v tabulke personalInfo
            modelBuilder.Entity<User>().HasOne(u => u.PersonalInfo).WithOne().HasForeignKey<User>(u => u.PersonalInfoIdUser).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<User>().HasOne(u => u.UserType).WithOne().HasForeignKey<User>(u => u.UserTypeIdUser).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TemporaryUser>().HasOne(tu => tu.PersonalInfo).WithOne().HasForeignKey<TemporaryUser>(tu => tu.PersonalInfoIdTempUser).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<TemporaryUser>().HasOne(tu => tu.UserType).WithOne().HasForeignKey<TemporaryUser>(tu => tu.UserTypeIdTempUser).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserType>().HasOne(ut => ut.ShippingAddress).WithOne().HasForeignKey<UserType>(ut => ut.ShippingAddressIdUser).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<UserType>().HasOne(ut => ut.BillingAddress).WithOne().HasForeignKey<UserType>(ut => ut.BillingAddressIdUser).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ShippingAddress>().HasOne(sa => sa.Address).WithOne().HasForeignKey<ShippingAddress>(sa => sa.AddressIdS).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<BillingAddress>().HasOne(ba => ba.Address).WithOne().HasForeignKey<BillingAddress>(ba => ba.AddressIdB).OnDelete(DeleteBehavior.Restrict);

        }
    }
}
