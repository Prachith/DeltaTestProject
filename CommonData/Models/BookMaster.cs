using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonData.Models
{
    public class BookMaster
    {
        public int Id { get; set; }
        public string BookName { get; set; }        
        public string Barcode { get; set; }
        public bool IsDeleted { get; set; }
        public int StockOnHand { get; set; }
        public decimal RentalCost {  get; set; }
        public int UserId { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastUpdated { get; set; }

    }
}
