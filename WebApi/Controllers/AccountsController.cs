using Microsoft.AspNetCore.Mvc;
using MvcBankAdmin.Models.DataMananger;
using MvcBankAdmin.Models;

namespace WebApi.Controllers
{

    //Account controller for webApt to get, put, update, delete data and update to database.
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly AccountManager _repo;

        // Inject the Account Manager to controller.
        public AccountsController(AccountManager repo)
        {
            _repo = repo;
        }

        // GETALL api/Accounts/1
        [HttpGet]
        public IEnumerable<Account> Get()
        {
            //Console.WriteLine(_repo.GetAll());
            return _repo.GetAll();
        }

        // GET api/Accounts/1
        [HttpGet("{id}")]
        public Account Get(int id)
        {
            return _repo.Get(id);
        }

        // ADD api/Accounts/1
        [HttpPost]
        public void Post([FromBody] Account account)
        {
            _repo.Add(account);
        }

        // UPDATE api/Accounts/1
        [HttpPut]
        public void Put([FromBody] Account account)
        {
            _repo.Update(account.AccountNumber, account);
        }

        // DELETE api/Accounts/1
        [HttpDelete("{id}")]
        public long Delete(int id)
        {
            return _repo.Delete(id);
        }
    }
}
