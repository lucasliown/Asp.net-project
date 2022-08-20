using MvcBankAdmin.Data;
using MvcBankAdmin.Models.Repository;

namespace MvcBankAdmin.Models.DataMananger
{
    //Login manager using Repository design patterns.
    public class LoginManager : IDataRepository<Login, string>
    {
        private readonly McbaContext _context;

        //Dependency inject the DB context.
        public LoginManager(McbaContext context)
        {
            _context = context;
        }

        //using EF core to add data to database.
        public string Add(Login item)
        {
            _context.Logins.Add(item);
            _context.SaveChanges();
            return item.LoginID ;
        }

        public string Delete(string id)
        {
            _context.Logins.Remove(_context.Logins.Find(id));
            _context.SaveChanges();
            return id;
        }

        public Login Get(string id)
        {
            return _context.Logins.Find(id);
        }

        public IEnumerable<Login> GetAll()
        {
            return _context.Logins.ToList();
        }

        public string Update(string id, Login item)
        {
            _context.Update(item);
            _context.SaveChanges();
            return id;
        }
    }
}
