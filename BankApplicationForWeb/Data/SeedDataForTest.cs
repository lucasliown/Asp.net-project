using BankApplicationForWeb.Models;

namespace BankApplicationForWeb.Data;

public static class SeedDataForTest
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        var contextForTest = serviceProvider.GetRequiredService<McbaContext>();

        Initialize(contextForTest);
    }
    // this method is used for test before
    public static void Initialize(McbaContext context)
    {
        
        if (context.Customers.Any())
            return; // DB has been seeded.
        var transcationsForAccount1 = new List<Transaction>()
        {
            new Transaction
            {
                TransactionType ="D",
                AccountNumber = 1,
                Amount=45,
                TransactionTimeUtc=DateTime.UtcNow,
            },
            new Transaction
            {
                TransactionType ="W",
                AccountNumber = 1,
                Amount=10,
                TransactionTimeUtc=DateTime.UtcNow,
            },
               new Transaction
            {
                TransactionType ="T",
                AccountNumber = 1,
                DestinationAccountNumber = 2,
                Amount=50,
                TransactionTimeUtc=DateTime.UtcNow,
            },
                new Transaction
            {
                TransactionType ="B",
                AccountNumber = 1,
                Amount=50,
                TransactionTimeUtc=DateTime.UtcNow,
            }
        };
        var transcationsForAccount2 = new List<Transaction>()
        {
            new Transaction
            {
                TransactionType ="D",
                AccountNumber = 2,
                Amount=45,
                TransactionTimeUtc=DateTime.UtcNow,
            },
            new Transaction
            {
                TransactionType ="W",
                AccountNumber = 2,
                Amount=10,
                TransactionTimeUtc=DateTime.UtcNow,
            },
               new Transaction
            {
                TransactionType ="T",
                AccountNumber = 2,
                DestinationAccountNumber = 2,
                Amount=50,
                TransactionTimeUtc=DateTime.UtcNow,
            },
                new Transaction
            {
                TransactionType ="B",
                AccountNumber = 2,
                Amount=50,
                TransactionTimeUtc=DateTime.UtcNow,
            }
        };


        context.Transactions.AddRange(transcationsForAccount1);
        context.Transactions.AddRange(transcationsForAccount2);



        var account1 = new List<Account>()
        {
            new Account
            {
                AccountNumber = 1,
                AccountType = "S",
                CustomerID = 1,
                Balance =100,
                Transactions = transcationsForAccount1,

            },
            new Account
            {
                AccountNumber = 2,
                AccountType = "C",
                CustomerID = 1,
                Balance =500,
                Transactions = transcationsForAccount2,

            },
        };


        var account2 = new List<Account>()
        {
            new Account
            {
                AccountNumber = 3,
                AccountType = "S",
                CustomerID = 2,
                Balance =400,


            },
            new Account
            {
                AccountNumber = 4,
                AccountType = "C",
                CustomerID = 2,
                Balance =500,


            },
        };

        context.Accounts.AddRange(account1);
        context.Accounts.AddRange(account2);

        var j = 0;
        context.Customers.AddRange(
            new Customer
            {
                CustomerID = 1,
                Name = "Rabbit",
                Accounts = account1,

            },
            new Customer
            {
                CustomerID = 2,
                Name = "Tiger",
                Accounts = account2,
            }
        );
        var billpay = new[]
        {
            new BillPay
            {
                AccountNumber= 1,
                PayeeId= 1,
                Amount=10,
                ScheduleTimeUtc= DateTime.UtcNow,
                Period="M",
                transactionStatus="P",
                LockState="L"

                },
               new BillPay
            {
                AccountNumber= 1,
                PayeeId= 2,
                Amount=20,
                ScheduleTimeUtc= DateTime.UtcNow,
                Period="O",
                transactionStatus="P",
                },
                      new BillPay
            {
                AccountNumber= 2,
                PayeeId= 3,
                Amount=20,
                ScheduleTimeUtc= DateTime.UtcNow,
                Period="O",
                transactionStatus="P",
                },

                 new BillPay
            {
                AccountNumber= 2,
                PayeeId= 3,
                Amount=20,
                ScheduleTimeUtc= DateTime.UtcNow,
                Period="O",
                transactionStatus="P",
                }
            };
        context.BillPay.AddRange(billpay);
        var payee = new[]
      {
            new Payee
            {
               Name ="test1",
               Address="here1",
               State="VIC",
               Postcode="3001",
               Phone="0426388894"
                },
              new Payee
            {
               Name ="test2",
               Address="here2",
               State="VIC",
               Postcode="3001",
               Phone="0426388899"
                },
                new Payee
            {
               Name ="test3",
               Address="here3",
               State="VIC",
               Postcode="3001",
               Phone="0426388890"
                },

        };
        context.Payee.AddRange(payee);





        context.SaveChanges();
    }
}
