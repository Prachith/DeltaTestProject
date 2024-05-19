using CommonData.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LibraryManagement.Models
{
    public class BookTransactionModel
    {

        public List<SelectListItem> Books { get; set; }
        public List<Transactions> Transactions { get; set; }

        public int BookId { get; set; }      

    }    
}
