using Microsoft.AspNetCore.Mvc;

namespace ElderlyService.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult LogIn()
        {
            return View();
        }
        public IActionResult Logout()
        {
            return RedirectToAction("Index","User");
        }
        public IActionResult Regesration()
        {
            return View();
        }

        public IActionResult RegisrationCaregiver()
        {
            return View();
        }
    }
}
