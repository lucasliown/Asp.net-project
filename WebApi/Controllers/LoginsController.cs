using Microsoft.AspNetCore.Mvc;
using MvcBankAdmin.Models.DataMananger;
using MvcBankAdmin.Models;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginsController : ControllerBase
    {
        private readonly LoginManager _repo;

        public LoginsController(LoginManager repo)
        {
            _repo = repo;
        }
        [HttpGet]
        public IEnumerable<Login> Get()
        {
            //Console.WriteLine(_repo.GetAll());
            return _repo.GetAll();
        }

        // GET api/Logins/1
        [HttpGet("{id}")]
        public Login Get(string id)
        {
            return _repo.Get(id);
        }

        // POST api/Logins
        [HttpPost]
        public void Post([FromBody] Login login)
        {
            _repo.Add(login);
        }

        // PUT api/Logins
        [HttpPut]
        public void Put([FromBody] Login login)
        {
            Console.WriteLine(1111111111111);
            _repo.Update(login.LoginID,login);
        }

        // DELETE api/Logins/1
        [HttpDelete("{id}")]
        public string Delete(string id)
        {
            return _repo.Delete(id);
        }
    }
}
