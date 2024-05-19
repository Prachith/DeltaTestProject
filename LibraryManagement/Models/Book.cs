using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Models
{
    public class Book
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter book name")]
        [Display(Name = "Name of the Book")]
        public string BookName { get; set; }
        [Required(ErrorMessage = "Please enter the barcode")]
        [Display(Name = "Barcode")]
        public string Barcode { get; set; }

        public bool IsDeleted { get; set; }
        [Required(ErrorMessage = "Please enter the stock on hand")]
        [Display(Name = "Stock On Hand")]
        public int StockOnHand { get; set; }

        [Required(ErrorMessage = "Please enter the Cost")]
        [Display(Name = "Rental Cost")]
        public decimal RentalCost { get; set; }
        public int UserId { get; set; }
        public List<Book>? Books { get; set; }
    }
}
