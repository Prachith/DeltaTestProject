using CommonData;
using CommonData.Models;
using LibraryManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Services
{
    public class BookService
    {
        private readonly DataContext _dbContext;
       
        public BookService(DataContext dBContext)
        {
            _dbContext = dBContext;            
        }

        public async Task<List<BookMaster>> GetBooks()
        {
            return await _dbContext.BookMasters.ToListAsync();
        }

        public async Task<Book> GetAllBooks()
        {
            var books = await GetBooks();
            var listBooks = books.Select(book => new Book
            {
                Id = book.Id,
                Barcode = book.Barcode,
                StockOnHand = book.StockOnHand,
                BookName = book.BookName,
                IsDeleted = book.IsDeleted,
                RentalCost = book.RentalCost
            }).ToList();

            return new Book { Books = listBooks };
        }

        public async Task<BookMaster?> GetBook(string barcode)
        {
            return await _dbContext.BookMasters.FirstOrDefaultAsync(x=>x.Barcode == barcode);            
        }

        public async Task<Book?> GetBook(int Id)
        {
            var bookMaster = await _dbContext.BookMasters.FirstOrDefaultAsync(x => x.Id == Id);
            if (bookMaster == null) return null;

            return new Book
            {
                Id = bookMaster.Id,
                Barcode = bookMaster.Barcode,
                BookName = bookMaster.BookName,
                IsDeleted = bookMaster.IsDeleted,
                StockOnHand = bookMaster.StockOnHand,
                RentalCost = bookMaster.RentalCost,
            };
        }

        public async Task<bool> AddBook(Book book, int sessionId)
        {
            var checkBook = await GetBook(book.Barcode);
            if (checkBook == null)
            {
                _dbContext.BookMasters.Add(new BookMaster
                {
                    Barcode = book.Barcode,
                    BookName = book.BookName,
                    IsDeleted = book.IsDeleted,
                    StockOnHand = book.StockOnHand,
                    RentalCost = book.RentalCost,
                    Created = DateTime.Now,
                    LastUpdated = DateTime.Now,
                    UserId = sessionId
                });
            }
            else
            {
                checkBook.StockOnHand = book.StockOnHand;
                checkBook.BookName = book.BookName;
                checkBook.RentalCost = book.RentalCost;
                checkBook.IsDeleted = book.IsDeleted;
                checkBook.LastUpdated = DateTime.Now;
                checkBook.UserId = sessionId;
            }
            await _dbContext.SaveChangesAsync();

            return true;
        }        


        public async Task<bool> DeleteBook(int id, int sessionId)
        {
            var book = await _dbContext.BookMasters.FindAsync(id);
            if(book != null)
            {
                book.IsDeleted = true;
                book.LastUpdated = DateTime.Now;
                book.UserId = sessionId;
                await _dbContext.SaveChangesAsync();
            }
            return true;
        }

        public async Task UpdateBookStock(List<Transactions> transactions, string transTpe)
        {
            foreach(Transactions transaction in transactions)
            {
                var book = await _dbContext.BookMasters.SingleOrDefaultAsync(x => x.Id == transaction.BookId);
                if (transTpe == "B") book.StockOnHand --;
                else book.StockOnHand ++;
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
