using BankApplicationForWeb.Data;
using BankApplicationForWeb.Models.ViewModel;


namespace BankApplicationForWeb.Models
{

    public class RealAccountService
    {
        private readonly McbaContext _context;
        private decimal WithdrawFee = 0.05m;
        private decimal TransFee = 0.10m;
        private decimal checkingAccountLeftMoney = 300;
        private decimal savingAccountLeftMoney = 0;

        public RealAccountService(McbaContext context)
        {
            _context = context;
        }
        //to compute the deposit  in the back-end
        public Transaction Deposit(int id, decimal amount, string? comment)
        {
            var transaction = new Transaction();
            var account = _context.Accounts.Find(id);
            account.Balance += amount;
            transaction.Comment = comment;
            transaction.Amount = amount;
            transaction.TransactionType = "D";
            transaction.TransactionTimeUtc = DateTime.UtcNow;
            account.Transactions.Add(transaction);
            _context.SaveChanges();
            return transaction;
        }

        //get all the account and change the view 
        //return a viewModel to the controller
        public async Task<ViewAccountTypeModel> GetRealAccount(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            ViewAccountTypeModel viewAccountTypeModel = new ViewAccountTypeModel();
            viewAccountTypeModel.AccountNumber = account.AccountNumber;
            if (account.AccountType.Equals("S"))
            {
                viewAccountTypeModel.AccountType = "Saving Account";
            }
            else
            {
                viewAccountTypeModel.AccountType = "Checking Account";
            }
            return viewAccountTypeModel;
        }

        //get the account depend on different account
        public decimal? GetBlance(int id)
        {
            var account = _context.Accounts.Find(id);
            var freeTrans = GetFreeTransaction(account);
            if (freeTrans >= 2)
            {
                if (account.AccountType.Equals("S"))
                {
                    return account.Balance - WithdrawFee;
                }
                else
                {
                    return account.Balance - WithdrawFee - checkingAccountLeftMoney;
                }
            }
            else
            {
                if (account.AccountType.Equals("S"))
                {
                    return account.Balance;
                }
                else
                {
                    return account.Balance - checkingAccountLeftMoney;
                }
            }
        }

        //doing the withdraw in the back-end 
        public List<Transaction> Withdraw(int id, decimal amount, string? comment)
        {
            var account = _context.Accounts.Find(id);
            account.Balance = account.Balance - amount;
            var freeTransactionTime = GetFreeTransaction(account);
            List<Transaction> transactions = new List<Transaction>();
            if (freeTransactionTime >= 2)
            {
                account.Balance = account.Balance - WithdrawFee;
                var transactionServiceChange = new Transaction();
                Console.WriteLine(transactionServiceChange.TransactionID);
                transactionServiceChange.TransactionType = "S";
                transactionServiceChange.Amount = WithdrawFee;
                transactionServiceChange.Comment = "Withdraw Service charge";
                transactionServiceChange.TransactionTimeUtc = DateTime.UtcNow;
                account.Transactions.Add(transactionServiceChange);
                Console.WriteLine(transactionServiceChange.TransactionID);
                transactions.Add(transactionServiceChange);
                Console.WriteLine(transactionServiceChange.TransactionID);
            }
            var transactionNormal = new Transaction();
            transactionNormal.TransactionType = "W";
            transactionNormal.Amount = amount;
            transactionNormal.Comment = comment;
            transactionNormal.TransactionTimeUtc = DateTime.UtcNow;
            Console.WriteLine(transactionNormal.TransactionID);
            account.Transactions.Add(
              transactionNormal);
            Console.WriteLine(transactionNormal.TransactionID);
            _context.SaveChanges();
            Console.WriteLine(transactionNormal.TransactionID);
            transactions.Add(transactionNormal);
            return transactions;
        }

        //get the all account that can be transfer
        public int GetAvailableTransToAccount(int fromAccount, int toAccount)
        {
            var Account = _context.Accounts.Find(toAccount);
            if (fromAccount == toAccount)
            {
                return 1;
            }
            else if (Account != null)
            {
                return 0;
            }
            else
            {
                return 2;
            }
        }

        //get all the balance of the account that you transfer
        public decimal? GetTransferBlance(int id)
        {
            var account = _context.Accounts.Find(id);
            var freeTrans = GetFreeTransaction(account);
            if (freeTrans >= 2)
            {
                if (account.AccountType.Equals("S"))
                {
                    return account.Balance - TransFee;
                }
                else
                {
                    return account.Balance - TransFee - checkingAccountLeftMoney;
                }
            }
            else
            {
                if (account.AccountType.Equals("S"))
                {
                    return account.Balance;
                }
                else
                {
                    return account.Balance - checkingAccountLeftMoney;
                }
            }

        }

        //doing the actual transfer bussiness rule in here
        public List<Transaction> Transfer(int id, int destinationAccount, decimal amount, string? comment)
        {
            List<Transaction> feedBackTransaction = new List<Transaction>();
            var fromAccount = _context.Accounts.Find(id);
            fromAccount.Balance = fromAccount.Balance - amount;
            var destinationCurrentAccount = _context.Accounts.Find(destinationAccount);
            destinationCurrentAccount.Balance += amount;
            var freeTransaction = GetFreeTransaction(fromAccount);
            Transaction transferTransaction = new Transaction();
            transferTransaction.TransactionType = "T";
            transferTransaction.DestinationAccountNumber = destinationAccount;
            transferTransaction.Amount = amount;
            transferTransaction.Comment = comment;
            transferTransaction.TransactionTimeUtc = DateTime.UtcNow;
            fromAccount.Transactions.Add(transferTransaction);

            destinationCurrentAccount.Transactions.Add(
                new Transaction
                {
                    TransactionType = "T",
                    Amount = amount,
                    TransactionTimeUtc = DateTime.UtcNow
                });

            if (freeTransaction >= 2)
            {
                fromAccount.Balance -= TransFee;
                Transaction transaction = new Transaction();
                transaction.TransactionType = "S";
                transaction.Amount = TransFee;
                transaction.Comment = "Transfer Service charge";
                transaction.AccountNumber = fromAccount.AccountNumber;
                transaction.TransactionTimeUtc = DateTime.UtcNow;
                fromAccount.Transactions.Add(transaction);
                feedBackTransaction.Add(transaction);
            }
            _context.SaveChanges();
            feedBackTransaction.Add(transferTransaction);
            return feedBackTransaction;
        }

        //get which transction can be free
        public int GetFreeTransaction(Account account)
        {
            int freeTransactionTime = 0;
            foreach (var trans in account.Transactions)
            {
                if (trans.TransactionType.Equals("W"))
                {
                    freeTransactionTime++;
                }
                else if (trans.TransactionType.Equals("T") && trans.DestinationAccountNumber != null)
                {
                    freeTransactionTime++;
                }
            }
            return freeTransactionTime;
        }
    }
}