using Microsoft.AspNetCore.Mvc;

namespace ElderlyService.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
