using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using MvcBankAdmin.Models;
using Newtonsoft.Json;

namespace MvcBankAdmin.Controllers
{
    //Home controller page to controll the home page.
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private HttpClient Client => _clientFactory.CreateClient("api");
        private readonly ILogger<HomeController> _logger;

        public HomeController(IHttpClientFactory clientFactory) => _clientFactory = clientFactory;

        //return the view for the home page
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}