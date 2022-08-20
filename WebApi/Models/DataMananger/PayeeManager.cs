using MvcBankAdmin.Data;
using MvcBankAdmin.Models.Repository;

namespace MvcBankAdmin.Models.DataMananger
{
    //Payee manager using Repository design patterns.
    public class PayeeManager : IDataRepository<Payee, int>
    {
        private readonly McbaContext _context;

        public PayeeManager(McbaContext context)
        {
            _context = context;
        }
        public int Add(Payee item)
        {
            _context.Payee.Add(item);
            _context.SaveChanges();
            return item.PayeeId;
        }

        public int Delete(int id)
        {
            _context.Payee.Remove(_context.Payee.Find(id));
            _context.SaveChanges();
            return id;
        }

        public Payee Get(int id)
        {
            return _context.Payee.Find(id);
        }

        public IEnumerable<Payee> GetAll()
        {
            //Console.WriteLine(_context.Customers.Find(2100).Accounts);
            return _context.Payee.ToList();
        }

        public int Update(int id, Payee item)
        {
            _context.Update(item);
            _context.SaveChanges();
            return id;
        }
    }
}
