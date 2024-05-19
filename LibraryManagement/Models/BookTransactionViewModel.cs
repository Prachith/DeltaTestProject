namespace LibraryManagement.Models
{
    public class BookTransactionViewModel
    {
        public int TransactionId { get; set; }
        public int BookId { get; set; }
        public int Quantity { get; set; }
        public string BookName { get; set; }
        public bool IsReturned { get; set; }
        
    }
}
