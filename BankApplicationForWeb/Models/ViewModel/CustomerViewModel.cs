namespace BankApplicationForWeb.Models.ViewModel
{
    //for display the customer
    public class CustomerViewModel  
    {
        public int CustomerID { get; set; }

        public string Name { get; set; }

        public string? TFN { get; set; }

        public string? Address { get; set; }

        public string? Suburb { get; set; }

        public string? State { get; set; }

        public string? PostCode { get; set; }

        public string? Mobile { get; set; }

        public virtual List<AccountViewModel>? Accounts { get; set; }
    }
}
