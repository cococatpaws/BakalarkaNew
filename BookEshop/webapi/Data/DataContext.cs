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
        public DbSet<ShippingType> ShippingType => Set<ShippingType>();
        public DbSet<PaymentType> PaymentType => Set<PaymentType>();
        public DbSet<Order> Order => Set<Order>();
        public DbSet<Order_Book> Orders_Books => Set<Order_Book>();


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

            modelBuilder.Entity<Order_Book>().HasKey(ob => new
            {
                ob.OrderId,
                ob.BookId
            });

            modelBuilder.Entity<Order_Book>().HasOne(ob => ob.Book).WithMany(b => b.OrdersBooks).HasForeignKey(ob => ob.BookId);
            modelBuilder.Entity<Order_Book>().HasOne(ob => ob.Order).WithMany(o => o.OrdersBooks).HasForeignKey(ob => ob.OrderId);


            //1 to 1 relationships
            //isRequired znamena, ze kazdy zaznam v tabulke user musi mat odpovedajuci zaznam v tabulke personalInfo
            modelBuilder.Entity<User>().HasOne(u => u.PersonalInfo).WithOne().HasForeignKey<User>(u => u.PersonalInfoIdUser).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<User>().HasOne(u => u.UserType).WithOne().HasForeignKey<User>(u => u.UserTypeIdUser).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TemporaryUser>().HasOne(tu => tu.PersonalInfo).WithOne().HasForeignKey<TemporaryUser>(tu => tu.PersonalInfoIdTempUser).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<TemporaryUser>().HasOne(tu => tu.UserType).WithOne().HasForeignKey<TemporaryUser>(tu => tu.UserTypeIdTempUser).OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<UserType>().HasOne(ut => ut.ShippingAddress).WithOne().HasForeignKey<UserType>(ut => ut.ShippingAddressIdUser).OnDelete(DeleteBehavior.Restrict);
            //modelBuilder.Entity<UserType>().HasOne(ut => ut.BillingAddress).WithOne().HasForeignKey<UserType>(ut => ut.BillingAddressIdUser).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ShippingAddress>().HasOne(sa => sa.Address).WithOne().HasForeignKey<ShippingAddress>(sa => sa.AddressIdS).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<BillingAddress>().HasOne(ba => ba.Address).WithOne().HasForeignKey<BillingAddress>(ba => ba.AddressIdB).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>().HasOne(o => o.ShippingAddress).WithMany(sa => sa.Orders).HasForeignKey(o => o.ShippingAddressIdO).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Order>().HasOne(o => o.BillingAddress).WithMany(ba => ba.Orders).HasForeignKey(o => o.BillingAddressIdO).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ShippingType>().HasData(
            new ShippingType { ShippingId = 1, ShippingTypeVar = webapi.Enums.ShippingType.GLS.ToString(), ShippingCost = 4.99 },
            new ShippingType { ShippingId = 2, ShippingTypeVar = webapi.Enums.ShippingType.Post.ToString(), ShippingCost = 4.49 },
            new ShippingType { ShippingId = 3, ShippingTypeVar = webapi.Enums.ShippingType.SPS.ToString(), ShippingCost = 4.49 },
            new ShippingType { ShippingId = 4, ShippingTypeVar = webapi.Enums.ShippingType.PersonalPickup.ToString(), ShippingCost = 0 }
            );

            modelBuilder.Entity<PaymentType>().HasData(
                new PaymentType { PaymentId = 1, PaymentTypeVar = webapi.Enums.PaymentType.CardTransfer.ToString(), AdditionalCost = 0},
                new PaymentType { PaymentId = 2, PaymentTypeVar = webapi.Enums.PaymentType.CashOnDelivery.ToString(), AdditionalCost = 1.50 }
                );

        }
    }
}
