using CommonData.Models;
using CommonData;
using LibraryManagement.Models;
using LibraryManagement.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject_Tests
{
    public class BookTransactionServiceTests
    {
        private DbContextOptions<DataContext> _dbContextOptions;

        public BookTransactionServiceTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "BookTransactionsTestDb")
                .Options;
        }

        private DataContext GetContext()
        {
            return new DataContext(_dbContextOptions);
        }

        [Fact]
        public async Task LoadTransaction_ByTransactionId()
        {
            // Arrange
            using (var context = GetContext())
            {                
                var bookTransactions = new List<BookTransactions>
                {
                    new BookTransactions { TransactionId = 1, BookId = 1, Quantity = 1, IsReturned = false, UserId = 2 },
                    new BookTransactions { TransactionId = 1, BookId = 2, Quantity = 1, IsReturned = false, UserId = 2 }
                };

                var book1 = new BookMaster { Id = 1, Barcode = "1234-1", BookName = "Test Book -1", StockOnHand = 10, IsDeleted = false };
                var book2 = new BookMaster { Id = 2, Barcode = "1234-2", BookName = "Test Book -2", StockOnHand = 5, IsDeleted = false };

                context.BookMasters.AddRange(book1, book2);
                context.BookTransactions.AddRange(bookTransactions);
                
                context.SaveChanges();

                var bookService = new BookService(context);
                var service = new BookTransactionsService(context, bookService);

                // Act
                var result = await service.LoadTransaction(1);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(2, result.Count);
                Assert.Equal("Test Book -1", result[0].BookName);
                Assert.Equal("Test Book -2", result[1].BookName);

            }
        }

        [Fact]
        public async Task LoadUserTransactions_ByUserId()
        {
            // Arrange
            using (var context = GetContext())
            {
                var userTransactions = new List<BookTransactions>
                {
                    new BookTransactions { TransactionId = 2, UserId = 1 },
                    new BookTransactions { TransactionId = 3, UserId = 1 }
                };

                context.BookTransactions.AddRange(userTransactions);
                context.SaveChanges();

                var bookService = new BookService(context);
                var service = new BookTransactionsService(context, bookService);

                // Act
                var result = await service.LoadUserTransactions(1);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(2, result.Borrowings.Count);
            }
        }

        [Fact]
        public async Task AddBookTransaction_Test()
        {
            // Arrange
            using (var context = GetContext())
            {
                var model = new BookTransactionModel
                {
                    Transactions = new List<Transactions>
                    {
                        new Transactions { BookId = 3, BookName="Test Book 3", Quantity = 1 }
                    }
                };

                var book1 = new BookMaster { Id = 3, Barcode = "1234-3", BookName = "Test Book -3", StockOnHand = 10, IsDeleted = false };
                
                context.BookMasters.Add(book1);
                var transactionNumber = 5;
                context.TransactionNumbers.Add(new TransactionNumbers { TransactionType = "B", TransactionId = transactionNumber });
                context.SaveChanges();

                var bookService = new BookService(context);
                var service = new BookTransactionsService(context, bookService);

                // Act
                var result = await service.AddBookTransaction(model, 3);

                // Assert
                Assert.NotNull(result);
                Assert.Equal($"Tranaction Number - {transactionNumber} Sucessfully Completed", result);
            }
        }

        [Fact]
        public async Task UpdateReturns_Test()
        {
            // Arrange
            using (var context = GetContext())
            {
                var bookTransaction = new BookTransactions
                {
                    TransactionId = 4,
                    BookId = 5,
                    Quantity = 1,
                    IsReturned = false
                };

                var book1 = new BookMaster { Id = 5, Barcode = "1234-5", BookName = "Test Book -5", StockOnHand = 10, IsDeleted = false };
                
                context.BookMasters.Add(book1);

                context.BookTransactions.Add(bookTransaction);
                context.SaveChanges();

                var bookService = new BookService(context);
                var service = new BookTransactionsService(context, bookService);

                // Act
                var result = await service.UpdateReturns(4, 5);

                // Assert
                Assert.True(result);
                var updatedTransaction = await context.BookTransactions
                                                      .FirstOrDefaultAsync(x => x.TransactionId == 4 && x.BookId == 5);
                Assert.True(updatedTransaction.IsReturned);
            }
        }

    }
}
