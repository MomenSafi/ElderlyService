using ElderlyService.Data;
using ElderlyService.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;

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

            string? userJson = HttpContext.Session.GetString("LiveUser");
            var user = JsonConvert.DeserializeObject<Users>(userJson);
            if(caregiver.Users.ImageFile==null)
            {
                caregiver.Users.ImageUrl = user.ImageUrl;
            }
            caregiver.Users.Password = user.Password;
            caregiver.Users.RoleId = user.RoleId;
            _db.Users.Update(caregiver.Users);
            _db.SaveChanges();
            caregiver.Users.ImageFile = null;
            HttpContext.Session.SetString("LiveUser", JsonConvert.SerializeObject(caregiver.Users));
            return RedirectToAction("CEditProfile", "Caregiver", new {id = caregiver.CaregiverId});
        }

        public IActionResult CEditProfile(string id)
        {
            var cargegiver = _db.Caregivers
               .Include(c => c.Service)
               .Include(c=>c.Users)
                   .AsNoTracking()

                .FirstOrDefault(c => c.CaregiverId == id);

            var services = _db.services.ToList();

            ViewBag.Services = new SelectList(services, "ServiceId", "Name");

            return View(cargegiver);
        }
        [HttpPost]
        public IActionResult CEditProfile([FromForm] Caregiver caregiver,int id)
        {

            caregiver.ServiceId = id;
            string? userJson = HttpContext.Session.GetString("LiveUser");
            var user = JsonConvert.DeserializeObject<Users>(userJson);
            caregiver.Users.Password = user.Password;
            caregiver.Users.RoleId= user.RoleId;
            caregiver.Users.Email = user.Email;
            caregiver.Users.FirstName = user.FirstName;
            caregiver.Users.LastName = user.LastName;
            caregiver.Users.Phone = user.Phone;
            caregiver.Users.City = user.City;
            caregiver.Users.DateOfBirth = user.DateOfBirth;
            caregiver.Users.ImageUrl = user.ImageUrl;
            _db.Caregivers.Update(caregiver);
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

            _db.SaveChanges();
            var newAvailableForThisWeek = _db.Availabilities.Where(c => c.CaregiverId == obj.CaregiverId).ToList();

            TempData["success"] = "Available time updated successfully";
            return RedirectToAction("CaregiverProfile", "User");
        }
    
       
        public IActionResult DeleteAvailable(int id)
        {
            Availability? obj = _db.Availabilities.Find(id);

            _db.Availabilities.Remove(obj);
            _db.SaveChanges();

            TempData["success"] = "Available time deleted and refilled successfully";
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

        public IActionResult DeleteExperiences(int id)
        {
            Experience? obj = _db.Experiences.Find(id);

            _db.Experiences.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = "Experience deleted successfully";
            return RedirectToAction("CaregiverProfile", "User");
        }

        public IActionResult Subscribe(string id)
        {
            var caregiver = _db.Caregivers.Find(id);
            ViewBag.Caregiver = caregiver;
            return View();
        }
        [HttpPost]
        public IActionResult Subscribe(CardData cardData, int Duration, string id)
        {
            var data = _db.Cards.ToList();
            var payment = new Payment();
            foreach (var card in data)
            {
                if (card.CardId == cardData.CardId && card.password == cardData.password)
                {
                    payment.Amount = Duration * 10;
                    payment.CaregiverId = id;
                    _db.Payments.Add(payment);
                    _db.SaveChanges();
                    TempData["success"] = "Your Payment done successfully";
                    return RedirectToAction("CaregiverProfile", "User");

                }
            }
            TempData["error"] = "Your CardId or Password is wrong";
            return RedirectToAction("Subscribe", new { id });
        }
        public IActionResult RejectAppointment(int id)
        {
            var appointment = _db.Appointments.Find(id);
            appointment.status = Appointment.Status.Rejected;
            _db.Appointments.Update(appointment);
            _db.SaveChanges();
            TempData["success"] = "this appointment rejected successfully";
            return RedirectToAction("CaregiverProfile", "User");

        }
        public IActionResult ApproveAppointment(int id)
        {
            var appointment = _db.Appointments.Find(id);
            appointment.status = Appointment.Status.Approved;
            _db.Appointments.Update(appointment);
            _db.SaveChanges();
            TempData["success"] = "this appointment approved successfully";
            return RedirectToAction("CaregiverProfile", "User");
        }
    }
}


