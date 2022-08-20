using Microsoft.AspNetCore.Mvc;
using System.Text;
using MvcBankAdmin.Models;
using Newtonsoft.Json;
using MvcBankAdmin.Web.Helper;
using System.Globalization;

namespace MvcBankAdmin.Controllers
{
    //BillPay controller for Admin Portal Website
    public class BillPaysController : Controller
    {
        //Factory for Client
        private readonly IHttpClientFactory _clientFactory;
        private HttpClient Client => _clientFactory.CreateClient("api");

        //Inject the Factory
        public BillPaysController(IHttpClientFactory clientFactory) => _clientFactory = clientFactory;

        //Index page for display all billpays
        public async Task<IActionResult> Index(int? id)
        {
            //Get data from webapi.
            var billpayResponse = await Client.GetAsync("api/BillPays");
            if (!billpayResponse.IsSuccessStatusCode)
                throw new Exception();
            var billpayResult = await billpayResponse.Content.ReadAsStringAsync();
            var billpays = JsonConvert.DeserializeObject<List<BillPayDto>>(billpayResult);

            //using view model to display the correct data on the web page.
            List<BillPayViewModel>? BillPay = new List<BillPayViewModel>();
            foreach (var billpay in billpays)
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
                    billPayViewModel.transactionStatus = "Pending";
                }
                else if (billpay.transactionStatus == "F")
                {
                    billPayViewModel.transactionStatus = "Transcation Failed";
                }
                else if (billpay.transactionStatus == "C")
                {
                    billPayViewModel.transactionStatus = "Completed";
                }

                BillPay.Add(billPayViewModel);
            }
            return View(BillPay);
        }

        //Lock bill pay function to lock the billpay.
        public async Task<IActionResult> LockBillPay(int? BillPayId)
        {
            if (BillPayId == null)
            {
                return NotFound();
            }

            //Get the billpay from the webapi through the specific ID.
            var response = await Client.GetAsync($"api/BillPays/{BillPayId}");
            if (!response.IsSuccessStatusCode)
                throw new Exception();
            var result = await response.Content.ReadAsStringAsync();
            var billPay = JsonConvert.DeserializeObject<BillPayDto>(result);
            billPay.LockState = "L";

            //up data the billpay state to webapi and also updata to database through webapi.
            if (ModelState.IsValid)
            {
                var content = new StringContent(JsonConvert.SerializeObject(billPay), Encoding.UTF8, "application/json");
                var billPayResponse = Client.PutAsync("api/BillPays", content).Result;
                //var response = CustomersApi.InitializeClient().PutAsync("api/Customers", content).Result;
                var BillPays = await GetBillPays();
                if (response.IsSuccessStatusCode)

                    return View("Index", BillPays);
            }
            //if the update failed return to the index page.
            var BillPay = await GetBillPays();
            return View("Index", BillPay);
        }

        //UnLock billpay function to unlock the billpay through admin portal.
        public async Task<IActionResult> UnLockBillPay(int? BillPayId)
        {
            if (BillPayId == null)
            {
                return NotFound();
            }
            //using Client to get billpay from webapi which is localhost:5000.
            var response = await Client.GetAsync($"api/BillPays/{BillPayId}");
            if (!response.IsSuccessStatusCode)
                throw new Exception();
            var result = await response.Content.ReadAsStringAsync();
            var billPay = JsonConvert.DeserializeObject<BillPayDto>(result);
            billPay.LockState = "U";

            //if the model state is valid update the data through webapi, using putAsync method.
            if (ModelState.IsValid)
            {
                var content = new StringContent(JsonConvert.SerializeObject(billPay), Encoding.UTF8, "application/json");
                var billPayResponse = Client.PutAsync("api/BillPays", content).Result;
                //var response = CustomersApi.InitializeClient().PutAsync("api/Customers", content).Result;
                var BillPays = await GetBillPays();
                if (response.IsSuccessStatusCode)
                    return View("Index", BillPays);
            }
            var BillPay = await GetBillPays();
            return View("Index", BillPay);
        }

        // Function get all billPays through webApi.
        public async Task<List<BillPayViewModel>> GetBillPays()
        {
            var billpayResponse = await Client.GetAsync("api/BillPays");
            if (!billpayResponse.IsSuccessStatusCode)
                throw new Exception();
            var billpayResult = await billpayResponse.Content.ReadAsStringAsync();
            var billpays = JsonConvert.DeserializeObject<List<BillPayDto>>(billpayResult);
            List<BillPayViewModel> BillPay = new List<BillPayViewModel>();
            foreach (var billpay in billpays)
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
                    billPayViewModel.transactionStatus = "Pending";
                }
                else if (billpay.transactionStatus == "F")
                {
                    billPayViewModel.transactionStatus = "Transcation Failed";
                }
                else if (billpay.transactionStatus == "C")
                {
                    billPayViewModel.transactionStatus = "Completed";
                }

                BillPay.Add(billPayViewModel);
            }
            return BillPay;
        }

    }
}
