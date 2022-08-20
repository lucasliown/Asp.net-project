using BankApplicationForWeb.Data;


namespace BankApplicationForWeb.Models
{

    public class RealCustomerService
    {
        private readonly McbaContext _context;

        public RealCustomerService(McbaContext context)
        {
            _context = context;

        }

        // get the all the customer in this service
        public Customer ShowAccountInCustomer(int CustomerID)
        {
            var showAccount = _context.Customers.Find(CustomerID);
            return showAccount;
        }

        //get the EFcore Account in the  back -end
        public Account GetEFAccountInCustomer(int accountNumber)
        {
            var Account = _context.Accounts.Find(accountNumber);
            return Account;
        }
    }
}
