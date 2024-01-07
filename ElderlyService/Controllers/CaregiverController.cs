using ElderlyService.Data;
using ElderlyService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ElderlyService.Controllers
{
    public class CaregiverController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CaregiverController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult EditProfile(string id)
        {
            var cargegiver = _db.Caregivers
                .Include(c => c.Users)
                   .ThenInclude(u => u.Roles)
               .Include(c => c.Service)
                .FirstOrDefault(c=>c.CaregiverId == id);

            var services = _db.services.ToList();

            ViewBag.Services = new SelectList(services, "ServiceId", "Name");

            return View(cargegiver);
        }
        [HttpPost]
        public IActionResult EditProfile(Caregiver caregiver)
        {
            _db.Caregivers.Update(caregiver);
            _db.Users.Update(caregiver.Users);
            _db.services.Update(caregiver.Service);
            _db.SaveChanges();
            return RedirectToAction("CaregiverProfile", "User");
        }
    }
}
