using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using CommonData;
using LibraryManagement.Services;
using CommonData.Models;
using LibraryManagement.Models;

namespace LibraryProject_Tests
{
    public class BookServiceTests
    {
        private DbContextOptions<DataContext> _dbContextOptions;

        public BookServiceTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "BookServiceTestDb")
                .Options;
        }

        [Fact]
        public async Task GetBooks_ByBarcode()
        {
            // Arrange
            using (var context = new DataContext(_dbContextOptions))
            {
                context.BookMasters.Add(new BookMaster { Id = 4, Barcode = "123", BookName = "Test Book -1", StockOnHand = 10 });
                context.SaveChanges();
            }

            using (var context = new DataContext(_dbContextOptions))
            {
                var service = new BookService(context);

                // Act
                var result = await service.GetBooks();

                // Assert
                //Assert.Single(result);
                Assert.Equal("123", result.First().Barcode);
            }
        }

        [Fact]
        public async Task AddNewBook()
        {
            // Arrange
            using (var context = new DataContext(_dbContextOptions))
            {
                var service = new BookService(context);
                var book = new Book { Barcode = "123", BookName = "Test Book -2", StockOnHand = 5, IsDeleted = false, RentalCost = 10 };

                // Act
                var result = await service.AddBook(book, 1);

                // Assert
                Assert.True(result);
                var addedBook = await context.BookMasters.FirstOrDefaultAsync(b => b.Barcode == "123");
                Assert.NotNull(addedBook);
                Assert.Equal("Test Book -2", addedBook.BookName);
            }
        }

        [Fact]
        public async Task DeleteBook()
        {
            // Arrange
            using (var context = new DataContext(_dbContextOptions))
            {
                context.BookMasters.Add(new BookMaster { Id = 2, Barcode = "1234", BookName = "Test Book -3", StockOnHand = 10, IsDeleted = false });
                context.SaveChanges();
            }

            using (var context = new DataContext(_dbContextOptions))
            {
                var service = new BookService(context);

                // Act
                var result = await service.DeleteBook(2, 1);

                // Assert
                Assert.True(result);
                var book = await context.BookMasters.FindAsync(2);
                Assert.NotNull(book);
                Assert.True(book.IsDeleted);
            }
        }

        [Fact]
        public async Task UpdateBookStock()
        {
            // Arrange
            using (var context = new DataContext(_dbContextOptions))
            {
                context.BookMasters.Add(new BookMaster { Id = 3, Barcode = "12345", BookName = "Test Book -4", StockOnHand = 10 });
                context.SaveChanges();
            }

            using (var context = new DataContext(_dbContextOptions))
            {
                var service = new BookService(context);
                var transactions = new List<Transactions>
                {
                    new Transactions { BookId = 3}
                };

                // Act
                await service.UpdateBookStock(transactions, "B");

                // Assert
                var book = await context.BookMasters.FindAsync(3);
                Assert.NotNull(book);
                Assert.Equal(9, book.StockOnHand);
            }
        }
    }
}