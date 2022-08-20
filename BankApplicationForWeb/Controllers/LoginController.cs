using Microsoft.AspNetCore.Mvc;
using BankApplicationForWeb.Data;
using BankApplicationForWeb.Models;


namespace BankApplicationForWeb.Controllers;


[Route("/Mcba/SecureLogin")]
public class LoginController : Controller
{
    private readonly McbaContext _context;
    private readonly RealBankService _RealBankService;
    public LoginController(McbaContext context, RealBankService RealBankService)
    {
        _context = context;
        _RealBankService = RealBankService;
    }
    //display the login page before log in 
    public IActionResult Login() => View();
    //to check the login in function to login in
    [HttpPost]
    public IActionResult Login(string loginID, string password)
    {

        var result = _RealBankService.GetLoginDetail(loginID, password);

        if (result.ContainsKey(1))
        {
            ModelState.AddModelError("LoginFailed", "Login failed, please try again.");
            return View(new Login { LoginID = loginID });
        }
        else if (result.ContainsKey(2))
        {
            foreach (var login in result.Values)
            {
                if (login.LockState == "L")
                {
                    return View("BlockedLogin");
                }
                HttpContext.Session.SetInt32(nameof(Customer.CustomerID), result[2].CustomerID);
                HttpContext.Session.SetString(nameof(Customer.Name), result[2].Customer.Name);
                HttpContext.Session.SetString(nameof(loginID), loginID);
            }
        }
        return RedirectToAction("ShowAccount", "Home");
    }

    [Route("LogoutNow")]
    public IActionResult Logout()
    {
        // Logout customer.
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }
}
