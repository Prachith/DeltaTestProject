using CommonData.Models;
using Microsoft.EntityFrameworkCore;

namespace CommonData
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : 
            base(options) 
        {
        }

        public DbSet<BookMaster> BookMasters { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<BookTransactions> BookTransactions { get; set; }
        public DbSet<TransactionNumbers> TransactionNumbers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookMaster>(entity =>
            {
                entity.ToTable("BookMaster");

                entity.Property(e => e.Id)
                .HasColumnName("Id")
                .HasColumnType("int");

                entity.Property(e => e.BookName)
                .HasColumnName("BookName")
                .HasColumnType("varchar(200)");

                entity.Property(e => e.Barcode)
                .HasColumnName("Barcode")
                .HasColumnType("varchar(100)");

                entity.Property(e => e.IsDeleted)
                .HasColumnName("IsDeleted")
                .HasColumnType("bit");

                entity.Property(e => e.StockOnHand)
                .HasColumnName("StockOnHand")
                .HasColumnType("int");

                entity.Property(e => e.RentalCost)
                .HasColumnName("RentalCost")
                .HasColumnType("money");

                entity.Property(e => e.UserId)
                .HasColumnName("UserId")
                .HasColumnType("int");

                entity.Property(e => e.Created)
                .HasColumnName("Created")
                .HasColumnType("datetime");

                entity.Property(e => e.LastUpdated)
                .HasColumnName("LastUpdated")
                .HasColumnType("datetime");

            });


            modelBuilder.Entity<Users>(entity =>
            {
                entity.ToTable("Users");

                entity.Property(e => e.Id)
                .HasColumnName("Id")
                .HasColumnType("int");

                entity.Property(e => e.UserId)
                .HasColumnName("UserID")
                .HasColumnType("varchar(20)");

                entity.Property(e => e.UserName)
                .HasColumnName("UserName")
                .HasColumnType("varchar(100)");

                entity.Property(e => e.Password)
                .HasColumnName("Password")
                .HasColumnType("varchar(50)");

                entity.Property(e => e.DateCreated)
                .HasColumnName("Created")
                .HasColumnType("datetime");

                entity.Property(e => e.DateUpdated)
                .HasColumnName("LastUpdated")
                .HasColumnType("datetime");

            });

            modelBuilder.Entity<BookTransactions>(entity =>
            {
                entity.ToTable("BookTransactions");
                
                entity.Property(e => e.Id)
                .HasColumnName("Id")
                .HasColumnType("int");

                entity.Property(e => e.TransactionId)
                .HasColumnName("TransactionId")
                .HasColumnType("int");

                entity.Property(e => e.BookId)
                .HasColumnName("BookId")
                .HasColumnType("int");

                entity.Property(e => e.Quantity)
                .HasColumnName("Quantity")
                .HasColumnType("int");

                entity.Property(e => e.UserId)
                .HasColumnName("UserId")
                .HasColumnType("int");
                                
                entity.Property(e => e.TransactionDate)
                .HasColumnName("TransactionDate")
                .HasColumnType("datetime");

                entity.Property(e => e.IsReturned)
                .HasColumnName("IsReturned")
                .HasColumnType("bit");

                entity.Property(e => e.DateReturned)
                .HasColumnName("DateReturned")
                .HasColumnType("datetime");
                
            });

            modelBuilder.Entity<TransactionNumbers>(entity =>
            {
                entity.ToTable("TransactionNumbers");
                
                entity.Property(e => e.Id)
                .HasColumnName("Id")
                .HasColumnType("int");

                entity.Property(e => e.TransactionId)
                .HasColumnName("TransactionId")
                .HasColumnType("int");

                entity.Property(e => e.TransactionType)
                .HasColumnName("TransactionType")
                .HasColumnType("char(1)");
            });
        }
    }
}
