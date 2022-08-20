using Microsoft.AspNetCore.Mvc;
using MvcBankAdmin.Models;


namespace MvcBankAdmin.Controllers;

// Bonus Material: Implement global authorisation check.
//[AllowAnonymous]
[Route("/Mcba/SecureLogin")]
public class LoginController : Controller
{
    //Login controller to controll the admin login,     LoginID:Admin    LoginPassword:Admin
    public IActionResult Login() => View();

    //Login method to valid the admin login by valid loginID and password.
    [HttpPost]
    public async Task<IActionResult> Login(string loginID, string password)
    {
        if (loginID != "Admin" || password != "Admin")
        {
            ModelState.AddModelError("LoginFailed", "Login failed, please try again.");
            return View(new LoginDto { LoginID = loginID });
        }
        else
        {
            //set session for loginID and Admin.
            HttpContext.Session.SetString("Admin","Admin");
            HttpContext.Session.SetString(nameof(loginID), loginID);
        }
        return RedirectToAction("Index", "Customers");
    }

    // Logout function.
    [Route("LogoutNow")]
    public IActionResult Logout()
    {
        // Logout Admin.
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }
}
