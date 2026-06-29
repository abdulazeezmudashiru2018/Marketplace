using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string Email, string Password)
        {
            if (Email == "admin@gmail.com" && Password == "12345")
            {
                return RedirectToAction("Index", "Dashboard");
            }

            ViewBag.Error = "Invalid Email or Password";

            return View();
        }
    }
    
}