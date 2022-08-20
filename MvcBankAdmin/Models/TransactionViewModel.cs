using System.ComponentModel.DataAnnotations;

namespace MvcBankAdmin.Models
{
    public class TransactionViewModel
    {
        //Transaction view model to display better information
        public int TransactionID { get; set; }

     
        public string TransactionType { get; set; }

     
        public int AccountNumber { get; set; }
       

        
        public int? DestinationAccountNumber { get; set; }
        

        
        public decimal Amount { get; set; }

       
        public string? Comment { get; set; }

        
        public string TransactionTimeUtc { get; set; }


    }
}
