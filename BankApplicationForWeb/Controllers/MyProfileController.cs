using Microsoft.AspNetCore.Mvc;
using BankApplicationForWeb.Data;
using BankApplicationForWeb.Models;
using BankApplicationForWeb.Utilities;
using BankApplicationForWeb.Models.ViewModel;

namespace BankApplicationForWeb.Controllers
{
    public class MyProfileController : Controller
    {
        private readonly McbaContext _context;

        private readonly RealBankService _RealBankService;
        // ReSharper disable once PossibleInvalidOperationException
        private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;
        private string loginID => HttpContext.Session.GetString(nameof(loginID));
        public MyProfileController(McbaContext context, RealBankService RealBankService)
        {
            _context = context;
            _RealBankService = RealBankService;
        }
        //display the all the customer data
        [HttpGet]
        public IActionResult Index()
        {
            var customer = _RealBankService.ShowAccountForModel(CustomerID);
            return View(customer);
        }

        //do the actuall data in the button and doing the data
        [HttpPost]
        public async Task<IActionResult> Index(string Name, string? TFN, string? address, string? suburb, string? state, string? postCode, string? mobile, string? password)
        {
            if (mobile != null)
            {
                char[] first = { mobile[0] };
                char[] second = { mobile[1] };
                string firstLetter = new string(first);
                string secondLetter = new string(second);
                if (firstLetter != "0" || secondLetter != "4")
                {
                    ModelState.AddModelError(nameof(mobile), "Mobile should start with 04");
                }
                if (mobile.Length != 10)
                {
                    ModelState.AddModelError(nameof(mobile), "Input should be of the format:04XX XXX XXX");
                }
            }
            if (state != null)
            {
                if (state.Length > 3)
                {
                    ModelState.AddModelError(nameof(state), "State should be a 2 or 3 lettered Australian state.");
                }
                else if (state.Length <= 1)
                {
                    ModelState.AddModelError(nameof(state), "State should be a 2 or 3 lettered Australian state.");
                }
            }
            if (postCode != null)
            {
                if (postCode.Length != 4)
                {
                    ModelState.AddModelError(nameof(postCode), "PostCode should be a 4 - digit number");
                }
            }
            if (!ModelState.IsValid)
            {
                return View(_context.Customers.Find(CustomerID));
            }
            if (password != null)
            {
                _RealBankService.ChangePassword(loginID, password);
            }
            //call the back-end service to change the profile
            _RealBankService.ChangeProfile(CustomerID, Name, TFN, address, suburb, state, postCode, mobile);
            return RedirectToAction("Index");
        }

    }
}
