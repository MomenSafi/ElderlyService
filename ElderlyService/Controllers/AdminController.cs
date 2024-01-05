using Microsoft.AspNetCore.Mvc;

namespace ElderlyService.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
