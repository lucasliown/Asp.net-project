using System.ComponentModel.DataAnnotations;

namespace MvcBankAdmin.Models
{
    // View model can diplay date.
    public class TransactionViewWithTime
    {
        public List<TransactionViewModel> Transaction { get; set; }
        [DataType(DataType.Date)]
        public DateTime startDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime endDate { get; set; }
    }
}
