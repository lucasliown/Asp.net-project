using Microsoft.AspNetCore.Mvc;
using System.Text;
using MvcBankAdmin.Models;
using Newtonsoft.Json;
using MvcBankAdmin.Web.Helper;
using System.Globalization;

namespace MvcBankAdmin.Controllers
{
    //Transaction controller to controll the transaction page for admin portal.
    public class TransactionsController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private HttpClient Client => _clientFactory.CreateClient("api");

        public TransactionsController(IHttpClientFactory clientFactory) => _clientFactory = clientFactory;

        // Index page to display transaction for the admin.
        // Using the input startDate, endDate.
        public async Task<ActionResult> Index(int? id, DateTime startDate, DateTime? endDate)
        {
            // Create view model to display transaction information.
            TransactionViewWithTime transactionViewWithTime = new TransactionViewWithTime();

            // Check if the endDate is null, change to use current date as endDate.
            if (endDate!=null) { } else { endDate = DateTime.Now; }
            var transactionResponse = await Client.GetAsync("api/Transactions");
            if (!transactionResponse.IsSuccessStatusCode)
                throw new Exception();
            var transactionResult = await transactionResponse.Content.ReadAsStringAsync();
            var transactions = JsonConvert.DeserializeObject<List<TransactionDto>>(transactionResult);
            List<TransactionViewModel> Transactions = new List<TransactionViewModel>();
            
            // Pass data to the view model.
            foreach (var transaction in transactions)
            {
                if (transaction.AccountNumber == id)
                {
                    if (transaction.TransactionTimeUtc>=startDate && transaction.TransactionTimeUtc<=endDate) {
                        TransactionViewModel trans = new TransactionViewModel();
                        trans.TransactionID = transaction.TransactionID;
                        trans.TransactionType = transaction.TransactionType;
                        trans.AccountNumber = transaction.AccountNumber;
                        trans.DestinationAccountNumber = transaction.DestinationAccountNumber;
                        trans.Amount = transaction.Amount;
                        trans.Comment = transaction.Comment;
                        trans.TransactionTimeUtc = transaction.TransactionTimeUtc.ToLocalTime().ToString("dd/MM/yyyy h:mm tt", new CultureInfo("en-AU"));
                        Transactions.Add(trans);
                    }
                }
            }
            transactionViewWithTime.Transaction = Transactions;
            return View(transactionViewWithTime);
        }
    }
}
