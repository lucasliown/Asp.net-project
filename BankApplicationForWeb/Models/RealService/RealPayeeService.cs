using BankApplicationForWeb.Data;
using Microsoft.EntityFrameworkCore;

namespace BankApplicationForWeb.Models
{

    public class RealPayeeService
    {
        private readonly McbaContext _context;

        public RealPayeeService(McbaContext context)
        {
            _context = context;
        }

        //get the payee in the back-end
        public async Task<List<Payee>> GetPayeesInPayee()
        {

            var payeesList = await _context.Payee.ToListAsync();
            return payeesList;
        }

        //get all the payyee by using async

        public List<Payee> GetPayeesInPayeeNotAsync()
        {
            var payeesList = _context.Payee.ToList();
            return payeesList;
        }
    }

}