namespace BankApplicationForWeb.Models.ViewModel
{
    //this is for display the transction
    public class TransactionViewModel
    {
        public int TransactionID { get; set; }

        public string TransactionType { get; set; }

        public int AccountNumber { get; set; }
    
        public string? DestinationAccountNumber { get; set; }
    
        public decimal Amount { get; set; }

        public string? Comment { get; set; }

        public string TransactionTimeUtc { get; set; }
    }
}