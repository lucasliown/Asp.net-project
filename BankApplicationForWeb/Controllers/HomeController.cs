using BankApplicationForWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using BankApplicationForWeb.Models.ViewModel;

namespace BankApplicationForWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly RealBankService _RealBankService;

        private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;
        
        public HomeController(ILogger<HomeController> logger, RealBankService RealBankService)
        {
            _logger = logger;
            _RealBankService = RealBankService;
        }

        public IActionResult Index()
        {
                return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        //this method is call the alll the account show in the home when you login 
        public IActionResult ShowAccount()
        {
            var customer = _RealBankService.ShowAccountForModel(CustomerID);
            string customerName=customer.Name;
            List<AccountViewModel>? Account = new List<AccountViewModel>();
            foreach(var account in customer.Accounts)
            {
                AccountViewModel AccountViewModel = new AccountViewModel();
                AccountViewModel.AccountNumber = account.AccountNumber;
               if (account.AccountType == "S")
                {
                    AccountViewModel.AccountType="Saving Account";
                }
                else
                {
                    AccountViewModel.AccountType = "Checking Account";
                }
                AccountViewModel.Balance = account.Balance;
                AccountViewModel.CustomerName = customerName;
                Account.Add(AccountViewModel);
            }
              
            return View(Account);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}