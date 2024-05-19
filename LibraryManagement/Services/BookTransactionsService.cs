using CommonData;
using CommonData.Models;
using LibraryManagement.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using static System.Reflection.Metadata.BlobBuilder;

namespace LibraryManagement.Services
{
    public class BookTransactionsService
    {
        private readonly DataContext _dbContext;
        private readonly BookService _bookService;
        
        public BookTransactionsService(DataContext dBContext, BookService bookService)
        {
            _dbContext = dBContext;
            _bookService = bookService;            
        }

        public async Task<BookTransactionModel> LoadBookTransactions()
        {
            var books = await LoadBooks();
            return new BookTransactionModel()
            {
                Books = books,
            };
        }

        public async Task<List<BookTransactionViewModel>> LoadTransaction(int? transactionId)
        {
            var bookTransactions = await _dbContext.BookTransactions
            .Where(x => x.TransactionId == transactionId)
            .ToListAsync();

            var listReturn = new List<BookTransactionViewModel>();

            foreach (var bookTransaction in bookTransactions)
            {
                var book = await _bookService.GetBook(bookTransaction.BookId);
                listReturn.Add(new BookTransactionViewModel()
                {
                    BookId = bookTransaction.BookId,
                    BookName = book.BookName,
                    IsReturned = bookTransaction.IsReturned,
                    Quantity = bookTransaction.Quantity,
                    TransactionId = bookTransaction.TransactionId
                });
            }

            return listReturn;
        }

        public async Task<BookReturnModel> LoadUserTransactions(int sessionId)
        {            
            var userTransactions = await _dbContext.BookTransactions
                                        .Where(x => x.UserId == sessionId)
                                        .GroupBy(x => x.TransactionId)
                                        .Select(g => g.First())
                                        .ToListAsync();

            var selectList = userTransactions
                .Select(transaction => new SelectListItem
                {
                    Text = transaction.TransactionId.ToString(),
                    Value = transaction.TransactionId.ToString()
                })
                .ToList();

            return new BookReturnModel
            {
                Borrowings = selectList
            };
        }

        public async Task<string> AddBookTransaction(BookTransactionModel model, int sessionId)
        {
            string response = string.Empty;
            
            if (model.Transactions.Count == 0) return "Invalid Transaction";

            int transactionNumber = await GetCurrentTransactionNumber("B");

            foreach (var bookTransaction in model.Transactions)
            {
                await _dbContext.BookTransactions.AddAsync(new BookTransactions()
                {
                    TransactionId = transactionNumber,
                    BookId = bookTransaction.BookId,
                    Quantity = bookTransaction.Quantity,
                    TransactionDate = DateTime.Now,
                    UserId = sessionId,
                    IsReturned = false,
                    DateReturned = DateTime.Now,
                });
            }
            await _dbContext.SaveChangesAsync();
            await _bookService.UpdateBookStock(model.Transactions, "B");
            await UpdateTransationNumber("B");

            if (transactionNumber == -1) response = $"Invalid Transaction";
            else response = $"Tranaction Number - {transactionNumber} Sucessfully Completed";

            return response;
        }

        public async Task UpdateTransationNumber(string type)
        {
            var currentId = await _dbContext.TransactionNumbers
            .SingleOrDefaultAsync(x => x.TransactionType == type);

            if (currentId != null)
            {
                currentId.TransactionId ++;
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<int> GetCurrentTransactionNumber(string type)
        {
            var transactionNumber = await _dbContext.TransactionNumbers
                .SingleOrDefaultAsync(x => x.TransactionType == type);
            return transactionNumber?.TransactionId ?? 0;
        }

        private async Task<List<SelectListItem>> LoadBooks()
        {
            var books = (await _bookService.GetBooks()).Where(x=>x.IsDeleted==false);
            return books.Select(book => new SelectListItem
            {
                Text = book.BookName,
                Value = book.Id.ToString()
            }).ToList();
        }

        public async Task<bool> UpdateReturns(int transactionId, int bookId)
        {
            var transaction = await _dbContext.BookTransactions
                    .FirstOrDefaultAsync(x => x.TransactionId == transactionId && x.BookId == bookId);

            if (transaction == null) return false;

            transaction.IsReturned = true;
            transaction.DateReturned = DateTime.Now;

            await _dbContext.SaveChangesAsync();

            var updateTransactions = new List<Transactions>
            {
                new Transactions
                {
                    BookId = bookId,
                    Quantity = transaction.Quantity,
                    BookName = string.Empty
                }
            };

            await _bookService.UpdateBookStock(updateTransactions, "R");

            return true;
        }


        public async Task<Transactions> AddBook(Transactions transactions)
        {
            var book =await _bookService.GetBook(transactions.BookId);
            
            return new Transactions()
            {
                BookId = book.Id,
                BookName = book.BookName,
                Quantity = 1
            };
        }
    }
}
