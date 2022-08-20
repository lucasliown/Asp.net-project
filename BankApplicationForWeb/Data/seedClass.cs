using BankApplicationForWeb.Models;
using Newtonsoft.Json;

namespace BankApplicationForWeb.Data
{
    public class seedClass
    {
        //seed the data and fill into the database when we are create the database
        public static void Initialize(IServiceProvider serviceProvider) {
            var context = serviceProvider.GetRequiredService<McbaContext>();
            if (context.Customers.Any())
                return;
            const string Url = "https://coreteaching01.csit.rmit.edu.au/~e103884/wdt/services/customers/";
            using var client = new HttpClient();
            var json = client.GetStringAsync(Url).Result;
            var customers = JsonConvert.DeserializeObject<List<Customer>>(json, new JsonSerializerSettings
            {
                // See here for DateTime format string documentation:
                // https://docs.microsoft.com/en-au/dotnet/standard/base-types/custom-date-and-time-format-strings
                DateFormatString = "yyyy/MM/dd hh:mm:ss"
            });
            foreach (var Customer in customers)
            {
                context.Customers.Add(Customer);
                foreach (var Account in Customer.Accounts)
                {
                    context.Accounts.Add(Account);
                    foreach (var Transaction in Account.Transactions)
                    {
                        Transaction.TransactionType = "D";
                        Transaction.AccountNumber = Account.AccountNumber;
                        Transaction.TransactionTimeUtc = Convert.ToDateTime(Transaction.TransactionTimeUtc);
                        Console.WriteLine(Transaction.TransactionTimeUtc);
                        context.Transactions.Add(Transaction);
                    }
                }
                var LoginJson = Customer.Login;
                LoginJson.CustomerID = Customer.CustomerID;
                context.Logins.Add(LoginJson);
             
            }
            context.SaveChanges();
      
        } 
    }
}
