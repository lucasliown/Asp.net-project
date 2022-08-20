using Microsoft.AspNetCore.Mvc;
using System.Text;
using MvcBankAdmin.Models;
using Newtonsoft.Json;
using MvcBankAdmin.Web.Helper;
using System.Globalization;

namespace MvcBankAdmin.Controllers
{
    //Customer controller to display and controll the all costomer.
    public class CustomersController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private HttpClient Client => _clientFactory.CreateClient("api");
        //Inject the ClientFactory.
        public CustomersController(IHttpClientFactory clientFactory) => _clientFactory = clientFactory;

        //index page for admin to dispaly all customer infromation.
        public async Task<IActionResult> Index()
        {
            var customerResponse = await Client.GetAsync("api/Customers");

            if (!customerResponse.IsSuccessStatusCode)
                throw new Exception();

            // Storing the response details received from web api.
            var custoemrResult = await customerResponse.Content.ReadAsStringAsync();

            // Deserializing the response received from web api and storing into a list.
            var customers = JsonConvert.DeserializeObject<List<CustomerDto>>(custoemrResult);

            var loginResponse = await Client.GetAsync("api/Logins");
            //var response = await MovieApi.InitializeClient().GetAsync("api/movies");
            if (!loginResponse.IsSuccessStatusCode)
                throw new Exception();
            // Storing the response details received from web api.
            var loginResult = await loginResponse.Content.ReadAsStringAsync();
            // Deserializing the response received from web api and storing into a list.
            var logins = JsonConvert.DeserializeObject<List<LoginDto>>(loginResult);
            //Match all login object to the specific customer.
            foreach (var customer in customers)
            {
                foreach (var login in logins)
                {
                    if (customer.CustomerID == login.CustomerID)
                    {
                        customer.Login = login;
                    }
                }
            }
            return View(customers);
        }

        //Edit function to edit the profile of the customer.
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();
            // Get specific customer from webApi.
            var response = await Client.GetAsync($"api/Customers/{id}");

            if (!response.IsSuccessStatusCode)
                throw new Exception();
            //DeserializeObject the json to customer object.
            var result = await response.Content.ReadAsStringAsync();
            var customer = JsonConvert.DeserializeObject<CustomerDto>(result);
            //return customer to view page.
            return View(customer);
        }

        //Edit function to edit profile and update the data to webAPi.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, CustomerDto customer)
        {
            //Check the customer id.
            if (id != customer.CustomerID)
                return NotFound();
            //input validation for the customer information.
            if (customer.Mobile != null)
            {
                char[] first = { customer.Mobile[0] };
                char[] second = { customer.Mobile[1] };
                string firstLetter = new string(first);
                string secondLetter = new string(second);
                if (firstLetter != "0" || secondLetter != "4")
                {
                    ModelState.AddModelError(nameof(customer.Mobile), "Mobile should start with 04");
                }
                if (customer.Mobile.Length != 10)
                {
                    ModelState.AddModelError(nameof(customer.Mobile), "Input should be of the format:04XX XXX XXX");
                }
            }

            // input validation for the customer state.
            if (customer.State != null)
            {
                if (customer.State.Length > 3)
                {
                    ModelState.AddModelError(nameof(customer.State), "State should be a 2 or 3 lettered Australian state.");
                }
                else if (customer.State.Length <= 1)
                {
                    ModelState.AddModelError(nameof(customer.State), "State should be a 2 or 3 lettered Australian state.");
                }
            }
            if (customer.PostCode != null)
            {
                if (customer.PostCode.Length != 4)
                {
                    ModelState.AddModelError(nameof(customer.PostCode), "PostCode should be a 4 - digit number");
                }
            }
            // if the information is correct, update the information to database through webApi.
            if (ModelState.IsValid)
            {
                var content = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json");
                var response = Client.PutAsync("api/Customers", content).Result;
                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Index");
            }
            return View(customer);
        }

        //Lock the login for a customer.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Lock(string LoginID)
        {
            //get all logins through webApi.
            var loginResponse = await Client.GetAsync("api/Logins");
            //var response = await MovieApi.InitializeClient().GetAsync("api/movies");
            if (!loginResponse.IsSuccessStatusCode)
                throw new Exception();
            // Storing the response details received from web api.
            var loginResult = await loginResponse.Content.ReadAsStringAsync();
            // Deserializing the response received from web api and storing into a list.
            var logins = JsonConvert.DeserializeObject<List<LoginDto>>(loginResult);
            LoginDto currentLogin = null;
            foreach (var login in logins)
            {
                if (login.LoginID == LoginID)
                {

                    currentLogin = login;
                    currentLogin.LockState = "L";
                    currentLogin.Customer = null;
                }
            }
            if (ModelState.IsValid)
            {
                var content = new StringContent(JsonConvert.SerializeObject(currentLogin), Encoding.UTF8, "application/json");
                var response = Client.PutAsync("api/Logins", content).Result;
                Console.WriteLine(content);
                //var response = CustomersApi.InitializeClient().PutAsync("api/Customers", content).Result;
                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        //UnLock function to unlock customer's login.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UnLock(string LoginID)
        {
            var loginResponse = await Client.GetAsync("api/Logins");
            //var response = await MovieApi.InitializeClient().GetAsync("api/movies");
            if (!loginResponse.IsSuccessStatusCode)
                throw new Exception();
            // Storing the response details received from web api.
            var loginResult = await loginResponse.Content.ReadAsStringAsync();
            // Deserializing the response received from web api and storing into a list.
            var logins = JsonConvert.DeserializeObject<List<LoginDto>>(loginResult);
            LoginDto currentLogin = null;
            foreach (var login in logins)
            {
                if (login.LoginID == LoginID)
                {

                    currentLogin = login;
                    currentLogin.LockState = "U";
                    currentLogin.Customer = null;
                }
            }
            if (ModelState.IsValid)
            {
                var content = new StringContent(JsonConvert.SerializeObject(currentLogin), Encoding.UTF8, "application/json");
                var response = Client.PutAsync("api/Logins", content).Result;
                Console.WriteLine(content);
                //var response = CustomersApi.InitializeClient().PutAsync("api/Customers", content).Result;
                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        //View account function, to view the account for different customer.
        public async Task<ActionResult> ViewAccount(int? id)
        {
            var accountResponse = await Client.GetAsync("api/Accounts");
            if (!accountResponse.IsSuccessStatusCode)
                throw new Exception();
            var accountResult = await accountResponse.Content.ReadAsStringAsync();
            var accounts = JsonConvert.DeserializeObject<List<AccountDto>>(accountResult);
            //Using view model to display account for admin page.
            List<AccountViewModelDto> Accounts = new List<AccountViewModelDto>();
            foreach (var account in accounts)
            {
                if (account.CustomerID == id)
                {
                    AccountViewModelDto AccountViewModel = new AccountViewModelDto();
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
                    AccountViewModel.CustomerID = account.CustomerID;
                    Accounts.Add(AccountViewModel);
                }
            }
            return View(Accounts);
        }

    }
}
