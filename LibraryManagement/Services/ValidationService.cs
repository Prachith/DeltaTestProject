using CommonData;
using LibraryManagement.Models;
using System.Numerics;

namespace LibraryManagement.Services
{
    public class ValidationService
    {
        private readonly BookTransactionsService _bookTransactionsService;
        
        public ValidationService(BookTransactionsService bookTransactionsService)
        {
            _bookTransactionsService = bookTransactionsService;            
        }

        public async Task<string> ValidateReturns(int transactionId, int bookId)
        {
            string response= String.Empty;

            var bookTransactions = (await _bookTransactionsService.LoadTransaction(transactionId)).FirstOrDefault(x=>x.BookId==bookId);

            if (bookTransactions == null) return response = "Invalid Book Transaction";

            if (bookTransactions.IsReturned) return response = "Book already returned!";

            var returns = await _bookTransactionsService.UpdateReturns(transactionId, bookId);
            if (returns) response = "Book Returned Sucessfully!";

            return response;
        }


        public string ValidateBook(Transactions model, BookTransactionModel transactionModel)
        {
            if (transactionModel.Transactions != null)
            {
                var book = transactionModel.Transactions.FirstOrDefault(x => x.BookId == model.BookId);

                if (book != null) return "Book Already Added! Choose a different book.";
                else return string.Empty;
            }
            else return string.Empty;
        }
    }
}
