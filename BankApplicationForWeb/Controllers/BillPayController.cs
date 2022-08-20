using Microsoft.AspNetCore.Mvc;
using BankApplicationForWeb.Data;
using BankApplicationForWeb.Models;
using BankApplicationForWeb.Models.ViewModel;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using BankApplicationForWeb.Utilities;

namespace BankApplicationForWeb.Controllers
{
    public class BillPayController : Controller
    {
        private readonly McbaContext _context;

        private readonly RealBankService _RealBankService;
        // ReSharper disable once PossibleInvalidOperationException
        private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;

        private int BillpayIDForSession => HttpContext.Session.GetInt32("BillPayId").Value;
        public BillPayController(McbaContext context, RealBankService RealBankService)
        {
            _context = context;
            _RealBankService = RealBankService;
        }
        //this method is used for display for the different type of Account 
        public IActionResult Index()
        {

            //var customer = _context.Customers.Find(CustomerID);
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

        // //this method is for view the billpay in the page
        public IActionResult ViewBillPay(int AccountNumber)
        {
            ViewBag.AccountNumber = AccountNumber;
            //this function is call the back-end get the full billpay list 
            var billpayList = _RealBankService.GetBillPay(AccountNumber);
            return View("ShowBillPay", billpayList);
        }


        public async Task<IActionResult> Create(int AccountNumber)
        {

            ViewBag.AccountNumber = AccountNumber;
            //this function is call the all the payee in the back-end
            var payeeList = await _RealBankService.GetPayee();
            var payeeNameList = payeeList.Select(x => x.Name).ToList();
            //return a view with the checkBox information 
            return View("Create", new ViewPayeeInCheckBox
            {
                ServiceName = "1",
                payeesNameList = new SelectList(payeeNameList),


            });
        }

        [HttpPost]
        public IActionResult CreateBillPay(int AccountNumber, ViewPayeeInCheckBox ViewPayeeInCheckBox)
        {
            //this can convert the local time become the utc time
            ViewPayeeInCheckBox.billPay.ScheduleTimeUtc = ViewPayeeInCheckBox.billPay.ScheduleTimeUtc.ToUniversalTime();
            ViewPayeeInCheckBox.billPay.AccountNumber = AccountNumber;
            var ID = 0;
            //this line use LinQ to get the specific payee data
            var payee = _context.Payee.Select(x => x).Where(x => x.Name == ViewPayeeInCheckBox.ServiceName).ToList();
            foreach (var payeeItem in payee)
            {
                ID = payeeItem.PayeeId;

            }
            ViewPayeeInCheckBox.billPay.PayeeId = ID;
            ViewPayeeInCheckBox.billPay.transactionStatus = "P";
            string messages = string.Join("; ", ModelState.Values
                                         .SelectMany(x => x.Errors)
                                         .Select(x => x.ErrorMessage));         
            //the code below is doing the check validation
            if (ViewPayeeInCheckBox.ServiceName == null)
            {
                ModelState.AddModelError("ServiceName", "The service can not be empty");
            }
            if (ViewPayeeInCheckBox.billPay.Amount <= 0)
                ModelState.AddModelError("billPay.Amount ", "Amount must be positive.");
            if (ViewPayeeInCheckBox.billPay.Amount.HasMoreThanTwoDecimalPlaces())
            {
                ModelState.AddModelError("billPay.Amount", "Amount cannot have more than 2 decimal places.");
            }
            if (!(ViewPayeeInCheckBox.billPay.Period == "M" || ViewPayeeInCheckBox.billPay.Period == "O"))
            {
                ModelState.AddModelError("billPay.Period", "Period Only can set to M or O.");
            }
            if (ViewPayeeInCheckBox.billPay.ScheduleTimeUtc <= DateTime.UtcNow)
            {
                ModelState.AddModelError("billPay.ScheduleTimeUtc", "Please give a correct Time.");
            }
            //if the enter value is false you will return the page agagin
            if (!ModelState.IsValid)
            {
                ViewBag.Amount = ViewPayeeInCheckBox.billPay.Amount;
                var payeeList = _RealBankService.GetPayeeNotAsync();
                var payeeNameList = payeeList.Select(x => x.Name).ToList();
                return View("Create", new ViewPayeeInCheckBox
                {
                    ServiceName = "1",
                    payeesNameList = new SelectList(payeeNameList),
                });
            }
            //if you are enter the correct data you can add the data in the database
            ViewPayeeInCheckBox.billPay.AccountNumber = AccountNumber;
            _RealBankService.AddNewBillPay(ViewPayeeInCheckBox.billPay);
            ViewBag.AccountNumber = AccountNumber;
            var billpayList = _RealBankService.GetBillPay(AccountNumber);
            return View("ShowBillPay", billpayList);
        }
        //this method is for cancel button to cancel the Billpay
        public IActionResult Cancel(int BillPayId, int AccountNumber)
        {
            _RealBankService.DeleteBillPay(BillPayId);
            ViewBag.AccountNumber = AccountNumber;
            var billpayList = _RealBankService.GetBillPay(AccountNumber);
            return View("ShowBillPay", billpayList);
        }
        //this method is use for show the modify billpay 
        public async Task<IActionResult> ModifyDetailPage(int BillPayId, int AccountNumber,int payeeID)
        {
            HttpContext.Session.SetInt32("BillPayId", BillPayId);
            ViewBag.AccountNumber = AccountNumber;
            ViewBag.PayyeID = payeeID;
            var payeeList = await _RealBankService.GetPayee();
            var payeeNameList = payeeList.Select(x => x.Name).ToList();
            var billpayFind = _RealBankService.FindBillPay(BillPayId);
            ViewPayeeInCheckBox viewPayeeInCheckBox = new ViewPayeeInCheckBox();
            viewPayeeInCheckBox.billPay=new BillPay();
            viewPayeeInCheckBox.billPay.BillPayId = BillPayId;
         
            viewPayeeInCheckBox.billPay.AccountNumber=AccountNumber;
            viewPayeeInCheckBox.billPay.PayeeId=billpayFind.PayeeId;
            viewPayeeInCheckBox.billPay.Amount=billpayFind.Amount;
            viewPayeeInCheckBox.billPay.ScheduleTimeUtc = billpayFind.ScheduleTimeUtc;
            viewPayeeInCheckBox.billPay.Period = billpayFind.Period;
            var billpayName=_RealBankService.FindBillPayName(billpayFind.PayeeId);
            viewPayeeInCheckBox.ServiceName = billpayName;
            viewPayeeInCheckBox.payeesNameList = new SelectList(payeeNameList);
            return View("EditBillPay", viewPayeeInCheckBox) ;
        }
        //this method is used for the actual edit the data
        public IActionResult  EditBillPay(int AccountNumber, ViewPayeeInCheckBox ViewPayeeInCheckBox)
        {
            Console.WriteLine(BillpayIDForSession);
            ViewPayeeInCheckBox.billPay.ScheduleTimeUtc = ViewPayeeInCheckBox.billPay.ScheduleTimeUtc.ToUniversalTime();
            ViewPayeeInCheckBox.billPay.AccountNumber = AccountNumber;
            var ID = 0;
            var payee = _context.Payee.Select(x => x).Where(x => x.Name == ViewPayeeInCheckBox.ServiceName).ToList();
            foreach (var payeeItem in payee)
            {
                ID = payeeItem.PayeeId;

            }
            ViewPayeeInCheckBox.billPay.PayeeId = ID;
            ViewPayeeInCheckBox.billPay.transactionStatus = "P";

            string messages = string.Join("; ", ModelState.Values
                                         .SelectMany(x => x.Errors)
                                         .Select(x => x.ErrorMessage));          
            //this code below is used for check vaildation
            if (ViewPayeeInCheckBox.ServiceName == null)
            {
                ModelState.AddModelError("ServiceName", "The service can not be empty");
            }
            if (ViewPayeeInCheckBox.billPay.Amount <= 0)
                ModelState.AddModelError("billPay.Amount ", "Amount must be positive.");
            if (ViewPayeeInCheckBox.billPay.Amount.HasMoreThanTwoDecimalPlaces())
            {
                ModelState.AddModelError("billPay.Amount", "Amount cannot have more than 2 decimal places.");
            }
            if (!(ViewPayeeInCheckBox.billPay.Period == "M" || ViewPayeeInCheckBox.billPay.Period == "O"))
            {
                ModelState.AddModelError("billPay.Period", "Period Only can set to M or O.");
            }
            if (ViewPayeeInCheckBox.billPay.ScheduleTimeUtc <= DateTime.UtcNow)
            {
                ModelState.AddModelError("billPay.ScheduleTimeUtc", "Please give a correct Time.");
            }
            if (!ModelState.IsValid)
            {                
                ViewBag.Amount = ViewPayeeInCheckBox.billPay.Amount;
                var payeeList = _RealBankService.GetPayeeNotAsync();
                var payeeNameList = payeeList.Select(x => x.Name).ToList();
                var billpayFind = _RealBankService.FindBillPay(BillpayIDForSession);
                
                ViewPayeeInCheckBox viewPayeeInCheckBoxRefresh = new ViewPayeeInCheckBox();
                viewPayeeInCheckBoxRefresh.billPay = new BillPay();
                viewPayeeInCheckBoxRefresh.billPay.BillPayId = BillpayIDForSession;
                viewPayeeInCheckBoxRefresh.billPay.AccountNumber = AccountNumber;
                viewPayeeInCheckBoxRefresh.billPay.PayeeId = billpayFind.PayeeId;
                viewPayeeInCheckBoxRefresh.billPay.Amount = billpayFind.Amount;
                viewPayeeInCheckBoxRefresh.billPay.ScheduleTimeUtc = billpayFind.ScheduleTimeUtc;
                viewPayeeInCheckBoxRefresh.billPay.Period = billpayFind.Period;
                var billpayName = _RealBankService.FindBillPayName(billpayFind.PayeeId);
                viewPayeeInCheckBoxRefresh.ServiceName = billpayName;
                viewPayeeInCheckBoxRefresh.payeesNameList = new SelectList(payeeNameList);
                return View("EditBillPay", viewPayeeInCheckBoxRefresh);             
            }
            ViewPayeeInCheckBox.billPay.AccountNumber = AccountNumber;
            _RealBankService.ChangeBillPay(ViewPayeeInCheckBox.billPay, BillpayIDForSession);
            ViewBag.AccountNumber = AccountNumber;
            var billpayList = _RealBankService.GetBillPay(AccountNumber);
            return View("ShowBillPay", billpayList);
        }
    }
}