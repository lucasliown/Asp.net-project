using Microsoft.AspNetCore.Mvc;
using BankApplicationForWeb.Data;
using BankApplicationForWeb.Models;
using BankApplicationForWeb.Utilities;
using BankApplicationForWeb.Models.ViewModel;
using System.Globalization;

namespace BankApplicationForWeb.Controllers
{
    public class WithdrawController : Controller
    {
        private readonly McbaContext _context;

        private readonly RealBankService _RealBankService;
        // ReSharper disable once PossibleInvalidOperationException
        private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;

        public WithdrawController(McbaContext context, RealBankService RealBankService)
        {
            _context = context;
            _RealBankService = RealBankService;
        }
        //display all the account detail in the website
        public IActionResult Index()
        {
            var customer = _RealBankService.ShowAccountForModel(CustomerID);
            string customerName = customer.Name;
            List<AccountViewModel>? Account = new List<AccountViewModel>();
            foreach (var account in customer.Accounts)
            {
                AccountViewModel AccountViewModel = new AccountViewModel();
                AccountViewModel.AccountNumber = account.AccountNumber;
                if (account.AccountType == "S")
                {
                    AccountViewModel.AccountType = "Saving Account";
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

        //display the withdraw enter box for display
        public async Task<IActionResult> Withdraw(int id)
        {
            var accountDisplay = await _RealBankService.GetAccount(id);
            return View(accountDisplay);
        }

        //doing the actually withdraw function in the back-end
        [HttpPost]
        public async Task<IActionResult> Withdraw(int id, decimal amount, string? comment)
        {
            var account = await _RealBankService.GetAccount(id);
            var balance = _RealBankService.GetAvailableBalance(id);
            //doing the data vaildation for the withdarw
            if (amount > balance)
            {
                ModelState.AddModelError(nameof(amount), "Do not have enough money. You Balance: " + balance);
            }
            if (amount <= 0)
                ModelState.AddModelError(nameof(amount), "Amount must be positive.");
            if (amount.HasMoreThanTwoDecimalPlaces())
                ModelState.AddModelError(nameof(amount), "Amount cannot have more than 2 decimal places.");
            if (comment != null)
            {
                if (comment.Length > 30)
                {
                    ModelState.AddModelError(nameof(comment), "Comment cannot more than 30 length.");
                }
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Amount = amount;
                ViewBag.Comment = comment;
                return View(account);
            }
            //call the back-end function to deploy the new transaction
            var transactions = _RealBankService.WithdrawInService(id, amount, comment);
            List<TransactionViewModel> transactionViewModelList = new List<TransactionViewModel>();
            foreach (var transactionData in transactions)
            {
                var time = transactionData.TransactionTimeUtc.ToLocalTime().ToString("dd/MM/yyyy h:mm tt", new CultureInfo("en-AU"));

                var type = "";
                if (transactionData.TransactionType == "W")
                {
                    type = "Withdraw";

                }
                else
                {
                    type = "Service Charge";
                }
                var checkDestinantionAccount = "";
                if (transactionData.DestinationAccountNumber == null)
                {
                    checkDestinantionAccount = "N/A";
                }
                else
                {
                    checkDestinantionAccount = transactionData.DestinationAccountNumber.ToString();
                }

                transactionViewModelList.Add(new TransactionViewModel
                {

                    TransactionTimeUtc = time,
                    TransactionID = transactionData.TransactionID,
                    TransactionType = type,
                    AccountNumber = transactionData.AccountNumber,
                    DestinationAccountNumber = checkDestinantionAccount,
                    Amount = transactionData.Amount,
                    Comment = transactionData.Comment,
                });
            }
            return View("ShowTranscation", transactionViewModelList);
        }

    }
}
