using Microsoft.AspNetCore.Mvc;
using MvcBankAdmin.Models.DataMananger;
using MvcBankAdmin.Models;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly CustomerManager _repo;

        public CustomersController(CustomerManager repo) { 
            _repo = repo;
        }
        [HttpGet]
        public IEnumerable<Customer> Get()
        {
            //Console.WriteLine(_repo.GetAll());
            return _repo.GetAll();
        }

        // GET api/Customers/1
        [HttpGet("{id}")]
        public Customer Get(int id)
        {
            return _repo.Get(id);
        }

        // POST api/Customers
        [HttpPost]
        public void Post([FromBody] Customer customer)
        {
            _repo.Add(customer);
        }

        // PUT api/Customers
        [HttpPut]
        public void Put(int id,[FromBody] Customer customer)
        {
            _repo.Update(id, customer);
        }

        // DELETE api/Customers/1
        [HttpDelete("{id}")]
        public long Delete(int id)
        {
            return _repo.Delete(id);
        }
    }
}
