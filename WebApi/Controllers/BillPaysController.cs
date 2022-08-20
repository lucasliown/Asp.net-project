using Microsoft.AspNetCore.Mvc;
using MvcBankAdmin.Models.DataMananger;
using MvcBankAdmin.Models;

namespace WebApi.Controllers
{
    // BillPay controller for webApi.
    [ApiController]
    [Route("api/[controller]")]
    public class BillPaysController : ControllerBase
    {
        private readonly BillPayManager _repo;

        // Inject the BillPayManager to controller.
        public BillPaysController(BillPayManager repo)
        {
            _repo = repo;
        }

        // GETALL api/BillPays/1
        [HttpGet]
        public IEnumerable<BillPay> Get()
        {
            return _repo.GetAll();
        }

        // GET api/BillPays/1
        [HttpGet("{id}")]
        public BillPay Get(int id)
        {
            return _repo.Get(id);
        }

        // ADD api/BillPays/1
        [HttpPost]
        public void Post([FromBody] BillPay billPay)
        {
            _repo.Add(billPay);
        }

        // UPDATE api/BillPays/1
        [HttpPut]
        public void Put([FromBody] BillPay billPay)
        {
            _repo.Update(billPay.BillPayId, billPay);
        }

        // DELETE api/BillPays/1
        [HttpDelete("{id}")]
        public int Delete(int id)
        {
            return _repo.Delete(id);
        }
    }
}
