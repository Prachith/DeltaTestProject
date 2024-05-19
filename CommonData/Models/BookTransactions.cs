using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonData.Models
{
    public class BookTransactions
    {
        public int Id { get; set; }
        public int TransactionId { get; set; }
        public int BookId { get; set; }
        public int Quantity { get; set; }
        public int UserId { get; set; }
        public DateTime TransactionDate { get; set; }
        public bool IsReturned { get; set; }
        public DateTime DateReturned { get; set; }
    }
}
