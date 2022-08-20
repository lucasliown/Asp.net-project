using Microsoft.AspNetCore.Mvc;
using BankApplicationForWeb.Data;
using BankApplicationForWeb.Models;
using BankApplicationForWeb.Utilities;
using BankApplicationForWeb.Filters;
using BankApplicationForWeb.Models.ViewModel;
using System.Globalization;

namespace McbaExample.Controllers;

// Can add authorize attribute to controllers.
[AuthorizeCustomer]
public class DepositController : Controller
{
    private readonly McbaContext _context;

    private readonly RealBankService _RealBankService;
    // ReSharper disable once PossibleInvalidOperationException
    private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;

    public DepositController(McbaContext context, RealBankService RealBankService)
    {
        _context = context;
        _RealBankService = RealBankService;
    }

  
    //this method is use for show the all account detetail
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
    //this method is dispay the deposit from
    public async Task<IActionResult> Deposit(int id)
    {
        var accountDisplay = await _RealBankService.GetAccount(id);
        return View(accountDisplay);
    }
    //this method is actual is do the deposit function 
    [HttpPost]
    public async Task<IActionResult> Deposit(int id, decimal amount, string? comment)
    {
        var account = await _RealBankService.GetAccount(id);
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
        var transaction = _RealBankService.DepositInService(id, amount, comment);
        var transactionViewModel = new TransactionViewModel();
        var checkDestinantionAccount = "";
        if (transaction.DestinationAccountNumber == null)
        {
            checkDestinantionAccount = "N/A";
        }
        else
        {
            checkDestinantionAccount = transaction.DestinationAccountNumber.ToString();
        }
        //create a new transaction and store in the database by calling the ef Core
        transactionViewModel.TransactionID = transaction.TransactionID;
        transactionViewModel.TransactionType = "Deposit";
        transactionViewModel.AccountNumber = transaction.AccountNumber;
        transactionViewModel.DestinationAccountNumber = checkDestinantionAccount;
        transactionViewModel.Amount = transaction.Amount;
        transactionViewModel.Comment = transaction.Comment;
        var time = transaction.TransactionTimeUtc;
        var timeTran = time.ToLocalTime().ToString("dd/MM/yyyy h:mm tt", new CultureInfo("en-AU")); 
        transactionViewModel.TransactionTimeUtc = timeTran;
        return View("ShowTranscation", transactionViewModel);
    }
}
