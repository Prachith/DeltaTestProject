using Microsoft.AspNetCore.Mvc.Rendering;

namespace LibraryManagement.Models
{
    public class BookReturnModel
    {
        public List<SelectListItem> Borrowings { get; set; }
        public List<BookTransactionViewModel> BookTransactions { get; set; }

        public int TransactionId { get; set; }

    }
}
