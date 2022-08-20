using MvcBankAdmin.Data;
using MvcBankAdmin.Models.Repository;

namespace MvcBankAdmin.Models.DataMananger
{

    //Account manager using Repository design patterns.
    public class AccountManager : IDataRepository<Account, int>
    {
        private readonly McbaContext _context;

        //Dependency Inject the database context.
        public AccountManager(McbaContext context)
        {
            _context = context;
        }

        //using saveChanges to update database.
        public int Add(Account item)
        {
            _context.Accounts.Add(item);
            _context.SaveChanges();
            return item.AccountNumber;
        }

        // using savechange to delete database.
        public int Delete(int id)
        {
            _context.Accounts.Remove(_context.Accounts.Find(id));
            _context.SaveChanges();
            return id;
        }

        public Account Get(int id)
        {
            return _context.Accounts.Find(id);
        }

        public IEnumerable<Account> GetAll()
        {
            //Console.WriteLine(_context.Customers.Find(2100).Accounts);
            return _context.Accounts.ToList();
        }

        public int Update(int id, Account item)
        {
            _context.Update(item);
            _context.SaveChanges();
            return id;
        }
    }
}
