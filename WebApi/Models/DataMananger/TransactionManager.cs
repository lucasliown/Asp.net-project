using MvcBankAdmin.Data;
using MvcBankAdmin.Models.Repository;

namespace MvcBankAdmin.Models.DataMananger
{
    //Transaction manager using Repository design patterns.
    public class TransactionManager : IDataRepository<Transaction, int>
    {
        private readonly McbaContext _context;

        //Dependency inject the DB context
        public TransactionManager(McbaContext context)
        {
            _context = context;
        }

        //Usng EF core to add data to database.
        public int Add(Transaction item)
        {
            _context.Transactions.Add(item);
            _context.SaveChanges();
            return item.TransactionID;
        }

        public int Delete(int id)
        {
            _context.Transactions.Remove(_context.Transactions.Find(id));
            _context.SaveChanges();
            return id;
        }

        public Transaction Get(int id)
        {
            return _context.Transactions.Find(id);
        }

        public IEnumerable<Transaction> GetAll()
        {
            //Console.WriteLine(_context.Customers.Find(2100).Accounts);
            return _context.Transactions.ToList();
        }

        public int Update(int id, Transaction item)
        {
            _context.Update(item);
            _context.SaveChanges();
            return id;
        }
    }
}
