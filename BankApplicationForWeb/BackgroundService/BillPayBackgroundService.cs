using BankApplicationForWeb.Data;
using Microsoft.EntityFrameworkCore;
using BankApplicationForWeb.Models;
namespace BankApplicationForWeb.BillPayBackgroundService;


public class BillPayBackgroundService : BackgroundService
{
    private readonly IServiceProvider _services;
    private readonly ILogger<BillPayBackgroundService> _logger;

    public BillPayBackgroundService(IServiceProvider services, ILogger<BillPayBackgroundService> logger)
    {

        _services = services;
        _logger = logger;
    }
    //this method is doing the method in the background every 4 second
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("BillPay Background Service is running.");

        while (!cancellationToken.IsCancellationRequested)
        {
            await DoWork(cancellationToken);

            _logger.LogInformation("BillPay Background Service is waiting a minute.");

            await Task.Delay(TimeSpan.FromSeconds(4), cancellationToken);
        }
    }

    //this method is the actural method for doing the staff in the background
    private async Task DoWork(CancellationToken cancellationToken)
    {
        _logger.LogInformation("BillPay Background Service is working.");

        using var scope = _services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<McbaContext>();
        var billpayList = await context.BillPay.ToListAsync(cancellationToken);
        DateTime checkTime = DateTime.UtcNow;
        if (billpayList.Count > 0)
        {

            foreach (var item in billpayList)
            {
                if (item.transactionStatus == "P")
                {

                    if (item.ScheduleTimeUtc <= checkTime)
                    {

                        if (item.LockState != "L")
                        {
                            //if the object can access this if the program can know this billpay can be process                           
                            if (item.Period == "M")
                            {

                                DateTime newTime = item.ScheduleTimeUtc.AddMonths(1);
                                Account account = context.Accounts.Where(x => x.AccountNumber == item.AccountNumber).FirstOrDefault();

                                if (account.AccountType == "S")
                                {
                                    if (account.Balance >= item.Amount)
                                    {
                                        account.Balance = account.Balance - item.Amount;
                                        Transaction transaction = new Transaction();
                                        transaction.TransactionType = "B";
                                        transaction.AccountNumber = item.AccountNumber;
                                        transaction.Amount = item.Amount;
                                        transaction.Comment = "BillPay is Completed";
                                        transaction.TransactionTimeUtc = checkTime;
                                        context.Transactions.Add(transaction);
                                        BillPay billPay = new BillPay();
                                        billPay.AccountNumber = item.AccountNumber;
                                        billPay.Amount = item.Amount;
                                        billPay.PayeeId = item.PayeeId;
                                        billPay.ScheduleTimeUtc = newTime;
                                        billPay.Period = item.Period;
                                        billPay.transactionStatus = "P";
                                        context.BillPay.Add(billPay);
                                        var billpayDBItem = context.BillPay.Find(item.BillPayId);
                                        billpayDBItem.transactionStatus = "C";
                                        await context.SaveChangesAsync(cancellationToken);
                                        _logger.LogInformation("BillPay Background Service secess to  process the BillPay.");
                                    }
                                    else
                                    {
                                        var billpayDBItem = context.BillPay.Find(item.BillPayId);
                                        billpayDBItem.transactionStatus = "F";
                                        await context.SaveChangesAsync(cancellationToken);
                                        _logger.LogInformation("BillPay Background Service Fail to  process the BillPay.");
                                    }
                                }
                                else if (account.AccountType == "C")
                                {
                                    if (account.Balance - 300 >= item.Amount)
                                    {
                                        account.Balance = account.Balance - item.Amount;
                                        Transaction transaction = new Transaction();
                                        transaction.TransactionType = "B";
                                        transaction.AccountNumber = item.AccountNumber;
                                        transaction.Amount = item.Amount;
                                        transaction.Comment = "BillPay is Completed";
                                        transaction.TransactionTimeUtc = checkTime;
                                        context.Transactions.Add(transaction);
                                        BillPay billPay = new BillPay();
                                        billPay.AccountNumber = item.AccountNumber;
                                        billPay.Amount = item.Amount;
                                        billPay.PayeeId = item.PayeeId;
                                        billPay.ScheduleTimeUtc = newTime;
                                        billPay.Period = item.Period;
                                        billPay.transactionStatus = "P";
                                        context.BillPay.Add(billPay);
                                        var billpayDBItem = context.BillPay.Find(item.BillPayId);
                                        billpayDBItem.transactionStatus = "C";
                                        await context.SaveChangesAsync(cancellationToken);
                                        _logger.LogInformation("BillPay Background Service secess to  process the BillPay.");
                                    }
                                    else
                                    {
                                        var billpayDBItem = context.BillPay.Find(item.BillPayId);
                                        billpayDBItem.transactionStatus = "F";
                                        await context.SaveChangesAsync(cancellationToken);
                                        _logger.LogInformation("BillPay Background Service Fail to  process the BillPay.");
                                    }
                                }
                            }
                            //if the data can accesss into this there are the one off billpay
                            else if (item.Period == "O")
                            {
                                DateTime newTime = item.ScheduleTimeUtc.AddMonths(1);
                                Account account = context.Accounts.Where(x => x.AccountNumber == item.AccountNumber).FirstOrDefault();
                                if (account.AccountType == "S")
                                {
                                    if (account.Balance >= item.Amount)
                                    {
                                        account.Balance = account.Balance - item.Amount;
                                        Transaction transaction = new Transaction();
                                        transaction.TransactionType = "B";
                                        transaction.AccountNumber = item.AccountNumber;
                                        transaction.Amount = item.Amount;
                                        transaction.Comment = "BillPay is Completed";
                                        transaction.TransactionTimeUtc = checkTime;
                                        context.Transactions.Add(transaction);
                                        var billpayDBItem = context.BillPay.Find(item.BillPayId);
                                        billpayDBItem.transactionStatus = "C";
                                        await context.SaveChangesAsync(cancellationToken);
                                        _logger.LogInformation("BillPay Background Service secess to  process the BillPay.");
                                    }
                                    else
                                    {
                                        var billpayDBItem = context.BillPay.Find(item.BillPayId);
                                        billpayDBItem.transactionStatus = "F";
                                        await context.SaveChangesAsync(cancellationToken);
                                        _logger.LogInformation("BillPay Background Service Fail to  process the BillPay.");
                                    }
                                }
                                if (account.AccountType == "C")
                                {
                                    if (account.Balance - 300 >= item.Amount)
                                    {
                                        account.Balance = account.Balance - item.Amount;
                                        Transaction transaction = new Transaction();
                                        transaction.TransactionType = "B";
                                        transaction.AccountNumber = item.AccountNumber;
                                        transaction.Amount = item.Amount;
                                        transaction.Comment = "BillPay is Completed";
                                        transaction.TransactionTimeUtc = checkTime;
                                        context.Transactions.Add(transaction);
                                        var billpayDBItem = context.BillPay.Find(item.BillPayId);
                                        billpayDBItem.transactionStatus = "C";
                                        await context.SaveChangesAsync(cancellationToken);
                                        _logger.LogInformation("BillPay Background Service secess to  process the BillPay.");
                                    }
                                    else
                                    {
                                        var billpayDBItem = context.BillPay.Find(item.BillPayId);
                                        billpayDBItem.transactionStatus = "F";
                                        await context.SaveChangesAsync(cancellationToken);
                                        _logger.LogInformation("BillPay Background Service Fail to  process the BillPay.");
                                    }
                                }
                            }
                        }
                    }
                    else if (item.ScheduleTimeUtc > checkTime)
                    {
                    }
                }
            }
        }
        else
        {
            _logger.LogInformation("BillPay Background Service didn't find any BillPay.");
        }
        _logger.LogInformation("BillPay Background Service work complete.");
    }
}

