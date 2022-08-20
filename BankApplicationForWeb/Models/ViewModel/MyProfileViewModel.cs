namespace BankApplicationForWeb.Models.ViewModel
{
    //display the profile 
    public class MyProfileViewModel
    {

        public int CustomerID { get; set; }
     
        public string Name { get; set; }

        public string? TFN { get; set; }

        public string? Address { get; set; }

        public string? Suburb { get; set; }

        public string? State { get; set; }

        public string? PostCode { get; set; }

        public string? Mobile { get; set; }

        public virtual List<Account>? Accounts { get; set; }

        public virtual Login Login { get; set; }
    }
}
