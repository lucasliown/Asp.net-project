using Microsoft.AspNetCore.Mvc;
using MvcBankAdmin.Models.DataMananger;
using MvcBankAdmin.Models;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly TransactionManager _repo;

        public TransactionsController(TransactionManager repo)
        {
            _repo = repo;
        }

        // GETALL api/Transaction/1
        [HttpGet]
        public IEnumerable<Transaction> Get()
        {
            //Console.WriteLine(_repo.GetAll());
            return _repo.GetAll();
        }

        [HttpGet("{id}")]
        public Transaction Get(int id)
        {
            return _repo.Get(id);
        }

        [HttpPost]
        public void Post([FromBody] Transaction transaction)
        {
            _repo.Add(transaction);
        }


        [HttpPut]
        public void Put([FromBody] Transaction transaction)
        {
            _repo.Update(transaction.TransactionID , transaction);
        }

        [HttpDelete("{id}")]
        public long Delete(int id)
        {
            return _repo.Delete(id);
        }
    }
}
