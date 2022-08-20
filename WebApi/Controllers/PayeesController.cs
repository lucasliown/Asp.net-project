using Microsoft.AspNetCore.Mvc;
using MvcBankAdmin.Models.DataMananger;
using MvcBankAdmin.Models;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PayeesController : ControllerBase
    {
        private readonly PayeeManager _repo;

        //Depend inject the payee manager.
        public PayeesController(PayeeManager repo)
        {
            _repo = repo;
        }

        // GETALL api/Payees
        [HttpGet]
        public IEnumerable<Payee> Get()
        {
            return _repo.GetAll();
        }

        // GET api/Payees/1
        [HttpGet("{id}")]
        public Payee Get(int id)
        {
            return _repo.Get(id);
        }


        // POST api/Payees
        [HttpPost]
        public void Post([FromBody] Payee payee)
        {
            _repo.Add(payee);
        }

        // PUT api/Payees
        [HttpPut]
        public void Put([FromBody] Payee payee)
        {
            _repo.Update(payee.PayeeId, payee);
        }

        // DELETE api/Payees/1
        [HttpDelete("{id}")]
        public int Delete(int id)
        {
            return _repo.Delete(id);
        }
    }
}
