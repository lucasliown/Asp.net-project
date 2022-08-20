using BankApplicationForWeb.Data;
using SimpleHashing;
using BankApplicationForWeb.Models.ViewModel;
using System.Globalization;

namespace BankApplicationForWeb.Models;

public class RealBankService
{
    public RealCustomerService Realcustomer { get; set; }


    public RealBillPayService RealBillPay { get; set; }

    public RealAccountService RealAccount { get; set; }
    public RealPayeeService RealPayee { get; set; }



    private readonly McbaContext _context;

    public RealBankService(McbaContext context)
    {
        _context = context;
        Realcustomer = new RealCustomerService(_context);
        RealAccount = new RealAccountService(_context);
        RealBillPay = new RealBillPayService();
        RealPayee = new RealPayeeService(_context);


    }

    //get the customer return to the Controller
    public Customer GetCustomer(int customerID)
    {
        var customer = _context.Customers.Find(customerID);
        return customer;
    }

    //call the method in the RealAccount and return a account to controller
    public async Task<ViewAccountTypeModel> GetAccount(int id)
    {
        var accountDisplay = await RealAccount.GetRealAccount(id);
        return accountDisplay;
    }

    //get the EF core Account for Controller
    public Account GetEFAccount(int AccountNumber)
    {
        var Account = Realcustomer.GetEFAccountInCustomer(AccountNumber);
        return Account;
    }

    //return a diction to check the vaildation in the back-end
    public Dictionary<int, Login> GetLoginDetail(string loginID, string password)
    {
        Dictionary<int, Login> result = new Dictionary<int, Login>();
        var login = _context.Logins.Find(loginID);
        if (login == null || string.IsNullOrEmpty(password) || !PBKDF2.Verify(login.PasswordHash, password))
        {
            result.Add(1, login);
            return result;
        }
        result.Add(2, login);
        return result;
    }

    //return a lot of account in the for display the account
    public Customer ShowAccountForModel(int CustomerID)
    {
        return Realcustomer.ShowAccountInCustomer(CustomerID);
    }

    //to get the actull billpay in the ef core and return the View model
    public List<BillPayViewModel> GetBillPay(int id)
    {
        List<BillPay> BillPayList = _context.BillPay.Where(x => x.AccountNumber == id).ToList();
        List<BillPayViewModel>? BillPay = new List<BillPayViewModel>();
        foreach (var billpay in BillPayList)
        {
            var time = billpay.ScheduleTimeUtc.ToLocalTime().ToLocalTime().ToString("dd/MM/yyyy h:mm tt", new CultureInfo("en-AU"));
            BillPayViewModel billPayViewModel = new BillPayViewModel();
            billPayViewModel.BillPayId = billpay.BillPayId;
            billPayViewModel.AccountNumber = billpay.AccountNumber;
            billPayViewModel.PayeeId = billpay.PayeeId;
            billPayViewModel.Amount = billpay.Amount;
            billPayViewModel.ScheduleTimeUtc = time;
            billPayViewModel.LockState = billpay.LockState;
            if (billpay.Period == "M")
            {
                billPayViewModel.Period = "Monthly";
            }
            else
            {
                billPayViewModel.Period = "One Off";
            }
            if (billpay.transactionStatus == "P")
            {
                billPayViewModel.Status = "Pending";
            }
            else if (billpay.transactionStatus == "F")
            {
                billPayViewModel.Status = "Transcation Failed";
            }
            else if (billpay.transactionStatus == "C")
            {
                billPayViewModel.Status = "Completed";
            }
            BillPay.Add(billPayViewModel);
        }
        return BillPay;
    }

    //return transation for the deposit
    public Transaction DepositInService(int id, decimal amount, string? comment)
    {
        var transcation = RealAccount.Deposit(id, amount, comment);
        return transcation;
    }
    //return transation for the withdraw
    public List<Transaction> WithdrawInService(int id, decimal amount, string? comment)
    {
        var transactions = RealAccount.Withdraw(id, amount, comment);
        return transactions;
    }

    //get the avalible transaction for account
    public int GetAvailableTransToAccount(int fromAccount, int destinationAccount)
    {
        var decisionForAccount = RealAccount.GetAvailableTransToAccount(fromAccount, destinationAccount);
        return decisionForAccount;
    }

    //get the avalible balance in the accountService
    public decimal? GetAvailableBalance(int id)
    {
        decimal? availableBalance = RealAccount.GetBlance(id);
        return availableBalance;
    }

    //get the avalible balance in the accountService
    public decimal? GetTransferAvailableBalance(int id)
    {
        decimal? availableBalance = RealAccount.GetTransferBlance(id);
        return availableBalance;
    }

    //get all the transaction for return
    public List<Transaction> Transfer(int id, int destinationAccount, decimal amount, string? comment)
    {
        var transactions = RealAccount.Transfer(id, destinationAccount, amount, comment);
        return transactions;
    }

    //use this method to change the password
    public void ChangePassword(string loginID, string? password)
    {
        var login = _context.Logins.Find(loginID);
        string hash = PBKDF2.Hash(password);
        login.PasswordHash = hash;
        _context.SaveChanges();
    }

    //change the profile in back-end 
    public void ChangeProfile(int customerID, string Name, string? TFN, string? address, string? suburb, string? state, string? postCode, string? mobile)
    {
        var customer = _context.Customers.Find(customerID);
        customer.Name = Name;
        customer.TFN = TFN;
        customer.Address = address;
        customer.Suburb = suburb;
        customer.State = state;
        customer.PostCode = postCode;
        customer.Mobile = mobile;
        _context.SaveChanges();
    }

    //add a new billpay in the back-end
    public void AddNewBillPay(BillPay billPay)
    {
        var PayyeeDB = _context.BillPay;
        PayyeeDB.Add(billPay);
        _context.SaveChanges();

    }

    //delete the billpay in the back-end
    public void DeleteBillPay(int billPayId)
    {
        BillPay DeleteItem = _context.BillPay.Find(billPayId);
        _context.BillPay.Remove(DeleteItem);
        _context.SaveChanges();
    }

    //get all the payyee in the back-end
    public async Task<List<Payee>> GetPayee()
    {
        var payeeList = await RealPayee.GetPayeesInPayee();
        return payeeList;
    }

    //to find the billpay in the database
    public BillPay FindBillPay(int billPayId)
    {
        var billpay = _context.BillPay.Find(billPayId);
        return billpay;
    }

    //get the payee by async
    public List<Payee> GetPayeeNotAsync()
    {
        var payeeList = RealPayee.GetPayeesInPayeeNotAsync();
        return payeeList;
    }

    //get the billpayee name in the back-end
    public string FindBillPayName(int PayeeId)
    {
        var name = _context.Payee.Where(x => x.PayeeId == PayeeId).Select(x => x.Name).FirstOrDefault();
        return name;
    }

    //change the billpay in the back-end
    public void ChangeBillPay(BillPay billPayFormWeb, int BillPayId)
    {
        Console.WriteLine("here" + BillPayId);
        var BillPayDB = _context.BillPay.Find(BillPayId);
        BillPayDB.PayeeId = billPayFormWeb.PayeeId;
        BillPayDB.Amount = billPayFormWeb.Amount;
        BillPayDB.ScheduleTimeUtc = billPayFormWeb.ScheduleTimeUtc;
        BillPayDB.Period = billPayFormWeb.Period;
        _context.SaveChanges();
    }

}

