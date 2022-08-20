using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankApplicationForWeb.Models
{


    public class RealTransactionService
    {
       
        public int TransactionID { get; set; }
 
        public string TransactionType { get; set; }
  
        public int AccountNumber { get; set; }
          
        public int? DestinationAccountNumber { get; set; }
       
        public decimal Amount { get; set; }
   
        public string? Comment { get; set; }
  
        public DateTime TransactionTimeUtc { get; set; }
    }
}