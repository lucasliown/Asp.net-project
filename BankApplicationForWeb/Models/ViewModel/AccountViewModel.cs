namespace BankApplicationForWeb.Models.ViewModel
{
    //for display account 
    public class AccountViewModel
    {

        public int AccountNumber { get; set; }
       
        public string AccountType { get; set; }

        public int CustomerID { get; set; }

        public string CustomerName { get; set; }    
 
        public decimal? Balance { get; set; }

    }
}
