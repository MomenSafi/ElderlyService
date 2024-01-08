using ElderlyService.Data;
using ElderlyService.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ElderlyService.Controllers
{
    public class CaregiverController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment webHostEnvironment;
        public CaregiverController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            this.webHostEnvironment = webHostEnvironment;
        }
        public IActionResult EditProfile(string id)
        {
            var cargegiver = _db.Caregivers
                .Include(c => c.Users)
                   .ThenInclude(u => u.Roles)
               .Include(c => c.Service)
                .FirstOrDefault(c => c.CaregiverId == id);

            var services = _db.services.ToList();

            ViewBag.Services = new SelectList(services, "ServiceId", "Name");

            return View(cargegiver);
        }
        [HttpPost]
        public IActionResult EditProfile([FromForm] Caregiver caregiver)
        {
            if (caregiver.Users.ImageFile != null)
            {
                string wwwRootPath = webHostEnvironment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + "" + caregiver.Users.ImageFile.FileName;
                string path = Path.Combine(wwwRootPath + "/Image/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    caregiver.Users.ImageFile.CopyTo(fileStream);
                }
                caregiver.Users.ImageUrl = "/Image/" + fileName;
            }
            _db.Caregivers.Update(caregiver);
            _db.Users.Update(caregiver.Users);
            _db.services.Update(caregiver.Service);
            _db.SaveChanges();
            return RedirectToAction("CaregiverProfile", "User");
        }

        //     Available
        public IActionResult AddAvailability(string id)
        {
            var caregiver = _db.Caregivers.Find(id);
            ViewBag.Caregiver = caregiver;
            return View();
        }
        [HttpPost]
        public IActionResult AddAvailability(Availability obj)
        {
            DayOfWeek dayOfWeek = obj.Date.DayOfWeek;
            obj.DayOfWeek = (Availability.Days)dayOfWeek;
            _db.Availabilities.Add(obj);
            var avaThisWeek = new AvilableForThisWeek
            {
                DayOfWeek = (AvilableForThisWeek.Days)obj.DayOfWeek,
                StartTime = obj.StartTime,
                EndTime = obj.EndTime,
                CaregiverId = obj.CaregiverId,
            };
            _db.AvilableForThisWeek.Add(avaThisWeek);
            _db.SaveChanges();
            TempData["success"] = "Available time Add successfully";
            return RedirectToAction("CaregiverProfile", "User");

        }
        public IActionResult EditAvailable(int? id)
        {
            Availability availability = _db.Availabilities.Find(id);
            return View(availability);
        }
        [HttpPost]
        public IActionResult EditAvailable(Availability obj)
        {
            _db.Availabilities.Update(obj);
            var avaThisWeek = new AvilableForThisWeek
            {
                DayOfWeek = (AvilableForThisWeek.Days)obj.DayOfWeek,
                StartTime = obj.StartTime,
                EndTime = obj.EndTime,
                CaregiverId = obj.CaregiverId,
            };
            _db.AvilableForThisWeek.Update(avaThisWeek);
            _db.SaveChanges();
            TempData["success"] = "Available time updated successfully";
            return RedirectToAction("CaregiverProfile", "User");

        }
        [HttpPost]
        public IActionResult DeleteAvailable(int id)
        {
            Availability? obj = _db.Availabilities.Find(id);
            var avaThisWeek = new AvilableForThisWeek
            {
                DayOfWeek = (AvilableForThisWeek.Days)obj.DayOfWeek,
                StartTime = obj.StartTime,
                EndTime = obj.EndTime,
                CaregiverId = obj.CaregiverId,
            };
            _db.Availabilities.Remove(obj);
            _db.AvilableForThisWeek.Remove(avaThisWeek);
            _db.SaveChanges();
            TempData["success"] = "Available time deleted successfully";
            return RedirectToAction("CaregiverProfile", "User");
        }


        //          Experiance


        public IActionResult AddExperience(string id)
        {
            var caregiver = _db.Caregivers.Find(id);
            ViewBag.Caregiver = caregiver;
            return View();
        }
        [HttpPost]
        public IActionResult AddExperience(Experience obj)
        {
            _db.Experiences.Add(obj);
            _db.SaveChanges();
            TempData["success"] = "Experience Add successfully";
            return RedirectToAction("CaregiverProfile", "User");

        }
        public IActionResult EditExperiences(int? id)
        {
            Experience experience = _db.Experiences.Find(id);
            return View(experience);
        }
        [HttpPost]
        public IActionResult EditExperiences(Experience obj)
        {
            _db.Experiences.Update(obj);
            _db.SaveChanges();
            TempData["success"] = "Experience updated successfully";
            return RedirectToAction("CaregiverProfile", "User");

        }
        [HttpPost]
        public IActionResult DeleteExperiences(int id)
        {
            Experience? obj = _db.Experiences.Find(id);
            
            _db.Experiences.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = "Experience deleted successfully";
            return RedirectToAction("CaregiverProfile", "User");
        }
    }
}


