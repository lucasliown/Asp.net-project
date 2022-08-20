using MvcBankAdmin.Data;
using MvcBankAdmin.Models.Repository;

namespace MvcBankAdmin.Models.DataMananger
{
    //Customer manager using Repository design patterns.
    public class CustomerManager : IDataRepository<Customer, int>
    {
        private readonly McbaContext _context;
        //Dependency inject the context
        public CustomerManager(McbaContext context) {
            _context = context;
        }
        //Add to database and save change using EF core.
        public int Add(Customer item)
        {
            _context.Customers.Add(item);
            _context.SaveChanges();
            return item.CustomerID;
        }

        // Delete data through the EF core.
        public int Delete(int id)
        {
            _context.Customers.Remove(_context.Customers.Find(id));
            _context.SaveChanges();
            return id;
        }

        public Customer Get(int id)
        {
            return _context.Customers.Find(id);
        }

        public IEnumerable<Customer> GetAll()
        {
            return _context.Customers.ToList();
        }

        public int Update(int id, Customer item)
        {
            _context.Update(item);
            _context.SaveChanges();
            return id;
        }
    }
}
