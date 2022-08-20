using Microsoft.AspNetCore.Mvc;
using BankApplicationForWeb.Data;
using BankApplicationForWeb.Models;
using BankApplicationForWeb.Utilities;
using BankApplicationForWeb.Filters;
using BankApplicationForWeb.Models.ViewModel;
using System.Globalization;

namespace BankApplicationForWeb.Controllers
{
    public class TransferController : Controller
    {
        private readonly McbaContext _context;

        private readonly RealBankService _RealBankService;
        // ReSharper disable once PossibleInvalidOperationException
        private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;

        public TransferController(McbaContext context, RealBankService RealBankService)
        {
            _context = context;
            _RealBankService = RealBankService;
        }
        //display all the account data in the page
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
        //dispaly the account detail for transfer
        public async Task<IActionResult> Transfer(int id)
        {
            var accountDisplay = await _RealBankService.GetAccount(id);
            return View(accountDisplay);
        }
        //doing the transfer function in the back-end
        [HttpPost]
        public async Task<IActionResult> Transfer(int id, int transToAccount, decimal amount, string? comment)
        {
            var account = await _RealBankService.GetAccount(id);
            var availableAccount = _RealBankService.GetAvailableTransToAccount(id, transToAccount);
            var balance = _RealBankService.GetTransferAvailableBalance(id);
            //doing the data vaildation in the controller
            if (transToAccount.ToString().Length > 4) {
                ModelState.AddModelError(nameof(transToAccount), "Destination Account Number cannot more than 4 length.");
            }
            if (availableAccount == 1)
            {
                ModelState.AddModelError(nameof(transToAccount), "Can not transfer to current account.");
            }
            else if (availableAccount == 2) {
                ModelState.AddModelError(nameof(transToAccount), "Do not have this account.");
            }
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
                ViewBag.TransToAccount = transToAccount;
                return View(account);
            }
            //display the transcation after you deploy the function
            var transactions = _RealBankService.Transfer(id, transToAccount, amount, comment);
            List<TransactionViewModel> transactionViewModelList = new List<TransactionViewModel>();
            foreach (var transactionData in transactions)
            {              
                var time = transactionData.TransactionTimeUtc.ToLocalTime().ToString("dd/MM/yyyy h:mm tt", new CultureInfo("en-AU"));
                var type = "";
                if (transactionData.TransactionType == "T")
                {
                    type = "Transfer";
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
            return View("ShowTranscation", transactionViewModelList); ;
        }

        }
}
