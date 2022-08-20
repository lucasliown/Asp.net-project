namespace BankApplicationForWeb.Models.ViewModel
{
    //for display the mystatement
    public class MyStatementAccountViewModel { 
    public int AccountNumber { get; set; }

    public string AccountType { get; set; }

    public int CustomerID { get; set; }

    public string CustomerName { get; set; }

    public decimal? Balance { get; set; }

    public virtual List<TransactionViewModel>? TransactionViewModel { get; set; }

    }
}

