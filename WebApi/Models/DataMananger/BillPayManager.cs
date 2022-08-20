using MvcBankAdmin.Data;
using MvcBankAdmin.Models.Repository;

namespace MvcBankAdmin.Models.DataMananger
{
    //BillPay manager using repository design.
    public class BillPayManager : IDataRepository<BillPay, int>
    {
        private readonly McbaContext _context;

        public BillPayManager(McbaContext context)
        {
            _context = context;
        }
        public int Add(BillPay item)
        {
            _context.BillPay.Add(item);
            _context.SaveChanges();
            return item.BillPayId;
        }

        public int Delete(int id)
        {
            _context.BillPay.Remove(_context.BillPay.Find(id));
            _context.SaveChanges();
            return id;
        }

        public BillPay Get(int id)
        {
            return _context.BillPay.Find(id);
        }

        public IEnumerable<BillPay> GetAll()
        {
            //Console.WriteLine(_context.Customers.Find(2100).Accounts);
            return _context.BillPay.ToList();
        }

        public int Update(int id, BillPay item)
        {
            _context.Update(item);
            _context.SaveChanges();
            return id;
        }
    }
}
