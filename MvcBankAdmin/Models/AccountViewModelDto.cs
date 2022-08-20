namespace MvcBankAdmin.Models
{
    public class AccountViewModelDto
    {
        // View model to display correct account information.
        public int AccountNumber { get; set; }

        public string AccountType { get; set; }

        public int CustomerID { get; set; }

        public string CustomerName { get; set; }    
 
        public decimal? Balance { get; set; }

    }
}
