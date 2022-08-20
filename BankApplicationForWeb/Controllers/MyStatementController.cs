using BankApplicationForWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using BankApplicationForWeb.Models.ViewModel;
using Newtonsoft.Json;
using X.PagedList;
using System.Globalization;

namespace BankApplicationForWeb.Controllers
{
    public class MyStateMentController : Controller
    {
        private const string SessionKey_Account = "MyStatementDisplay";

        private readonly RealBankService _RealBankService;
        //store the customer data in the session
        private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;

        public MyStateMentController( RealBankService RealBankService)
        {
            
            _RealBankService = RealBankService;
        }
        //display all the accont in the page
        public IActionResult Index()
        {
            var customer=_RealBankService.GetCustomer(CustomerID);
            List < AccountViewModel > AccountViewModel=new List<AccountViewModel>();
            foreach(var account in customer.Accounts)
            {
                AccountViewModel accountViewModels = new AccountViewModel();
                accountViewModels.AccountNumber = account.AccountNumber;
                accountViewModels.Balance = account.Balance;
                if (account.AccountType == "S") {
                    accountViewModels.AccountType = "Saving Account";
                    
                }
                else if (account.AccountType =="C")
                {
                    accountViewModels.AccountType = "Checking Account";
                }
                accountViewModels.CustomerID=customer.CustomerID;
                accountViewModels.CustomerName = customer.Name;
                AccountViewModel.Add(accountViewModels);
            }
           
            return View(AccountViewModel);
        }
        //transfer the all data into the next controller method 
        [HttpPost]
        public IActionResult IndexToViewStateMent(int AccountNumber)
        {         
            var Account = _RealBankService.GetEFAccount(AccountNumber);           
            if (Account == null)
                return NotFound();         
            MyStatementAccountViewModel MyStateAccount = new MyStatementAccountViewModel();
            MyStateAccount.AccountNumber = Account.AccountNumber;
            MyStateAccount.Balance = Account.Balance;
            if (Account.AccountType == "S")
            {
                MyStateAccount.AccountType = "Saving Account";
            }
            else
            {
                MyStateAccount.AccountType = "Checking Account";
            }
            MyStateAccount.CustomerID = Account.CustomerID;
            MyStateAccount.TransactionViewModel = new List<TransactionViewModel>();
            foreach (var transcation in Account.Transactions)
            {
                TransactionViewModel transactionView = new TransactionViewModel();
                var time = transcation.TransactionTimeUtc.ToLocalTime().ToString("dd/MM/yyyy h:mm tt",new CultureInfo("en-AU"));
                var type = "";
                if (transcation.TransactionType == "T")
                {
                    type = "Transfer";

                }else if(transcation.TransactionType == "W")
                {
                    type = "WithDraw";
                }
                else if (transcation.TransactionType == "D")
                {
                    type = "Deposit";
                }
                else if (transcation.TransactionType == "S")
                {
                    type = "Service Charge";
                }
                else if (transcation.TransactionType == "B")
                {
                    type = "BillPay";
                }

                var checkDestinantionAccount = "";
                if (transcation.DestinationAccountNumber == null)
                {
                    checkDestinantionAccount = "N/A";
                }
                else
                {
                    checkDestinantionAccount = transcation.DestinationAccountNumber.ToString();
                }
                TransactionViewModel transactionViewModelForShow = new TransactionViewModel();
                transactionViewModelForShow.TransactionTimeUtc = time;
                transactionViewModelForShow.TransactionID = transcation.TransactionID;
                transactionViewModelForShow.TransactionType = type;
                transactionViewModelForShow.AccountNumber = transcation.AccountNumber;
                transactionViewModelForShow.DestinationAccountNumber = checkDestinantionAccount;
                transactionViewModelForShow.Amount = transcation.Amount;
                transactionViewModelForShow.Comment = transcation.Comment;
                MyStateAccount.TransactionViewModel.Add(transactionViewModelForShow);
            }

            
            // Store a complex object in the session via JSON serialisation.
            var MyStateAccountJson = JsonConvert.SerializeObject(MyStateAccount);
                    
            HttpContext.Session.SetString(SessionKey_Account, MyStateAccountJson);

            return RedirectToAction(nameof(ViewMyStatement));
        }

        //display the data as a page list X.PagedList pageage
        public IActionResult ViewMyStatement(int? page=1 )
        {         
            var MyStateAccountJson = HttpContext.Session.GetString(SessionKey_Account);
            if (MyStateAccountJson == null)
                return RedirectToAction(nameof(Index)); // OR return BadRequest();

            // Retrieve complex object from the session via JSON deserialisation.
            var AccountForStatement = JsonConvert.DeserializeObject<MyStatementAccountViewModel>(MyStateAccountJson);
            ViewBag.Account = AccountForStatement;
            var pagedList = AccountForStatement.TransactionViewModel.OrderByDescending
             (x => x.TransactionTimeUtc).ToPagedList((int)page, 4);
            return View(pagedList);
        }

        public IActionResult Privacy() => View();


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}