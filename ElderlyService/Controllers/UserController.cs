using ElderlyService.Data;
using ElderlyService.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq;
using ElderlyService.SendEmails;

namespace ElderlyService.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IEmailService _emailService;
        public UserController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment, IEmailService emailService)
        {
            _db = db;
            this.webHostEnvironment = webHostEnvironment;
            _emailService = emailService;
        }

        public class ServiceAndTestimonials
        {
            public List<Service> Services { get; set; }
            public List<ReviewsForWebsites> ReviewsForWebsitess { get; set; }
        }

        public IActionResult Index()
        {
            ServiceAndTestimonials serviceAndTestimonials = new ServiceAndTestimonials
            {
                Services = _db.services.ToList(),
                ReviewsForWebsitess = _db.reviewsForWebsites
                .Include(t => t.Users)
                .Where(s => s.status == ReviewsForWebsites.Status.Approved).ToList()
            };

            ViewBag.ProfessionalCaregiver = _db.Caregivers.Count(v => v.Valid == true);
            ViewBag.ServiceWeGive = _db.services.Count();
            ViewBag.UserUseWebsite = _db.Users.Count(r => r.RoleId == "3");
            ViewBag.HappyPeople = _db.Reviews.Count(r => r.Rate >= 3);

            return View(serviceAndTestimonials);
        }



        public IActionResult Service()
        {
            var service = _db.services.ToList();
            return View(service);
        }

        public IActionResult Caregiver(int? id, string? SearchItem, string? selectedDayOfWeek, string? startTime, string? endTime)
        {
            List<Caregiver> caregiver;
            caregiver = _db.Caregivers
                .Include(c => c.Users)
                .Include(c => c.Service)
                .Include(c => c.Reviews)
                .Include(c => c.Availabilities)
                .Where(c => c.Valid == true)
                .ToList();
            if (id != null)
            {
                caregiver = caregiver.Where(s => s.ServiceId == id).ToList();
            }
            if (!string.IsNullOrEmpty(SearchItem))
            {
                caregiver = caregiver
                    .Where(c => c.Users != null && (c.Users.FirstName.Contains(SearchItem) || c.Users.LastName.Contains(SearchItem)))
                    .ToList();
            }
            if (!string.IsNullOrEmpty(selectedDayOfWeek))
            {
                caregiver = caregiver
                    .Where(c => c.Availabilities != null && c.Availabilities.Any(a => selectedDayOfWeek.Contains(a.DayOfWeek.ToString())))
                    .ToList();
            }
            if (!string.IsNullOrEmpty(startTime) && !string.IsNullOrEmpty(endTime))
            {
                DateTime start = DateTime.Today.Add(TimeSpan.Parse(startTime));
                DateTime end = DateTime.Today.Add(TimeSpan.Parse(endTime));

                caregiver = caregiver
                    .Where(c => c.Availabilities.Any(a => a.StartTime.TimeOfDay >= start.TimeOfDay && a.EndTime.TimeOfDay <= end.TimeOfDay))
                    .ToList();
            }
            caregiver = caregiver.OrderByDescending(c => c.Reviews.Any() ? c.Reviews.Average(r => r.Rate) : 0).ToList();
            return View(caregiver);
        }

        public IActionResult Testimonials()
        {
            var testimonials = _db.reviewsForWebsites
                .Include(t => t.Users)
                .Where(t => t.status == ReviewsForWebsites.Status.Approved).ToList();
            return View(testimonials);
        }

        public IActionResult AddTestimonials(string comment)
        {
            string? userJson = HttpContext.Session.GetString("LiveUser");
            var user = JsonConvert.DeserializeObject<Users>(userJson);
            var testimonial = new ReviewsForWebsites();
            testimonial.Comment = comment;
            testimonial.userId = user.userId;
            _db.reviewsForWebsites.Add(testimonial);
            _db.SaveChanges();

            TempData["info"] = "Your testimonial has been submitted for review. Thank you for your feedback!";
            return RedirectToAction("Testimonials");
        }

        public IActionResult About()
        {

            ViewBag.ProfessionalCaregiver = _db.Caregivers.Count(v => v.Valid == true);
            ViewBag.ServiceWeGive = _db.services.Count();
            ViewBag.UserUseWebsite = _db.Users.Count(r => r.RoleId == "3");
            ViewBag.HappyPeople = _db.Reviews.Count(r => r.Rate >= 3);

            // Retrieve caregivers with their average rating
            var caregiversWithRating = _db.Caregivers
                .Where(c => c.Valid == true)
                .Include(c => c.Reviews) // Include Reviews navigation property
                .Include(c => c.Users)
                .Include(c => c.Service)
                .Where(c => c.Reviews.Any()) // Only include caregivers with at least one review
                .Select(c => new
                {
                    Caregiver = c,
                    AverageRating = c.Reviews.Average(r => r.Rate)
                })
                .OrderByDescending(c => c.AverageRating)
                .ToList();

            // Extract the Caregiver objects from the anonymous type
            var TopRattingCaregivers = caregiversWithRating.Select(c => c.Caregiver).ToList();



            return View(TopRattingCaregivers);
        }


        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Profile(string id)
        {
            var caregiver = _db.Caregivers
                .Include(c => c.Users)
                    .ThenInclude(u => u.Roles)
                .Include(c => c.Reviews.Where(r => r.status == Reviews.Status.Approved))
                    .ThenInclude(r => r.Users)
                .Include(c => c.Service)
                .Include(c => c.Experiences)
                .Include(c => c.Availabilities.Where(a => a.Status == 1))
                .FirstOrDefault(c => c.CaregiverId == id);
            return View(caregiver);
        }

        public IActionResult MyProfile()
        {
            TempData.Remove("ReturnUrl");
            string? userJson = HttpContext.Session.GetString("LiveUser");
            if(userJson == null)
            {
                TempData["error"] = "You must Login first";
                TempData["ReturnUrl"] = Url.Action("MyProfile", "User");
                return RedirectToAction("LogIn", "Login");
            }
            var user = JsonConvert.DeserializeObject<Users>(userJson);
            if (user.RoleId == "2")
            {
                return RedirectToAction("CaregiverProfile");
            }
            else
            {
                return RedirectToAction("UserProfile");
            }

        }
        public IActionResult CaregiverProfile()
        {
            string? userJson = HttpContext.Session.GetString("LiveUser");
            if (userJson == null)
            {
                TempData["error"] = "You must Login first";
                TempData["ReturnUrl"] = Url.Action("MyProfile", "User");
                return RedirectToAction("LogIn", "Login");
            }
            var user = JsonConvert.DeserializeObject<Users>(userJson);
            var caregiver = _db.Caregivers
                .Include(c => c.Users)
                   .ThenInclude(u => u.Roles)
               .Include(c => c.Reviews.Where(r => r.status == Reviews.Status.Approved))
                    .ThenInclude(r => r.Users)
               .Include(c => c.Service)
               .Include(c => c.Experiences)
               .Include(c => c.Availabilities.Where(a => a.Status == 1))
               .Include(c => c.Appointments)
                    .ThenInclude(a => a.Users)
                .FirstOrDefault(c => c.Users.userId == user.userId);
            return View(caregiver);
        }
        [HttpPost]
        public IActionResult CaregiverProfile(string SearchItem)
        {
            string? userJson = HttpContext.Session.GetString("LiveUser");
            var user = JsonConvert.DeserializeObject<Users>(userJson);

            if (!string.IsNullOrEmpty(SearchItem))
            {
                var filteredCaregiver = _db.Caregivers
                    .Include(c => c.Users)
                        .ThenInclude(u => u.Roles)
                    .Include(c => c.Reviews.Where(r => r.status == Reviews.Status.Approved))
                        .ThenInclude(r => r.Users)
                    .Include(c => c.Service)
                    .Include(c => c.Experiences)
                    .Include(c => c.Availabilities.Where(a => a.Status == 1))
                    .Include(c => c.Appointments)
                        .ThenInclude(a => a.Users)
                    .FirstOrDefault(c => c.Users.userId == user.userId);

                if (filteredCaregiver != null)
                {
                    filteredCaregiver.Appointments = filteredCaregiver.Appointments
                        .Where(a => a.Users.FirstName.Contains(SearchItem) || a.Users.LastName.Contains(SearchItem))
                        .ToList();

                    return View(filteredCaregiver);
                }
            }

            var caregiver = _db.Caregivers
                .Include(c => c.Users)
                    .ThenInclude(u => u.Roles)
                .Include(c => c.Reviews.Where(r => r.status == Reviews.Status.Approved))
                    .ThenInclude(r => r.Users)
                .Include(c => c.Service)
                .Include(c => c.Experiences)
                .Include(c => c.Availabilities.Where(a => a.Status == 1))
                .Include(c => c.Appointments)
                    .ThenInclude(a => a.Users)
                .FirstOrDefault(c => c.Users.userId == user.userId);

            return View(caregiver);
        }
        public IActionResult UserProfile()
        {
            string? userJson = HttpContext.Session.GetString("LiveUser");
            if (userJson == null)
            {
                TempData["error"] = "You must Login first";
                TempData["ReturnUrl"] = Url.Action("MyProfile", "User");
                return RedirectToAction("LogIn", "Login");
            }
            var user = JsonConvert.DeserializeObject<Users>(userJson);
            var us = _db.Users
                .Include(u => u.Appointments)
                    .ThenInclude(a => a.Caregiver)
                        .ThenInclude(c => c.Users)

                .SingleOrDefault(u => u.userId == user.userId);

            return View(us);
        }

        public IActionResult EditProfile(string id)
        {
            string? userJson = HttpContext.Session.GetString("LiveUser");
            var user = JsonConvert.DeserializeObject<Users>(userJson);
            return View(user);
        }

        [HttpPost]
        public IActionResult EditProfile(Users user)
        {
            if (user.ImageFile != null)
            {
                string wwwRootPath = webHostEnvironment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + "" + user.ImageFile.FileName;
                string path = Path.Combine(wwwRootPath + "/Image/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    user.ImageFile.CopyTo(fileStream);
                }
                user.ImageUrl = "/Image/" + fileName;
            }
            string? userJson = HttpContext.Session.GetString("LiveUser");
            var thisuser = JsonConvert.DeserializeObject<Users>(userJson);
            if (user.ImageFile == null)
            {
                user.ImageUrl = thisuser.ImageUrl;
            }
            user.Password = thisuser.Password;
            user.RoleId = thisuser.RoleId;
            user.Email = thisuser.Email;
            _db.Users.Update(user);
            _db.SaveChanges();
            user.ImageFile = null;
            HttpContext.Session.SetString("LiveUser", JsonConvert.SerializeObject(user));
            return RedirectToAction("UserProfile", "User");
        }


        public IActionResult AddReview(string message, int rate, string Id)
        {
            string? userJson = HttpContext.Session.GetString("LiveUser");
            var user = JsonConvert.DeserializeObject<Users>(userJson);
            var review = new Reviews();
            review.userId = user.userId;
            review.CaregiverId = Id;
            review.Rate = rate;
            review.Comment = message;

            _db.Reviews.Add(review);
            _db.SaveChanges();
            TempData["info"] = "Your testimonial has been submitted for review. Thank you for your feedback!";
            return RedirectToAction("Profile", new { id = Id });
        }

        public IActionResult Appointment(string id)
        {
            Caregiver? caregiver = _db.Caregivers
                .Include(c => c.Users)
                .FirstOrDefault(c => c.CaregiverId == id);
            return View(caregiver);
        }
        [HttpPost]
        public async Task<IActionResult> Appointment(string id, DateTime StartTime, DateTime EndTime, DateTime BookingDate, string Location, string Notes)
        {
            var appointment = new Appointment();
            appointment.StartTime = StartTime;
            appointment.EndTime = EndTime;
            appointment.status = Models.Appointment.Status.pending;
            appointment.BookingDate = BookingDate;
            appointment.DayOfWeek = (Appointment.Days)BookingDate.DayOfWeek;
            appointment.Notes = Notes;
            appointment.Location = Location;
            appointment.CaregiverId = id;
            string? userJson = HttpContext.Session.GetString("LiveUser");
            var user = JsonConvert.DeserializeObject<Users>(userJson);
            appointment.userId = user.userId;
            _db.Appointments.Add(appointment);
            _db.SaveChanges();
            TempData["success"] = "Appointment apply successfully, Wait to reply it";
            var caregiver = _db.Caregivers
                .Where(c => c.CaregiverId == appointment.CaregiverId)
                .Include(c => c.Users).FirstOrDefault();
            string caregiverEmail = caregiver.Users.Email;
            string subject = "Appointment Request";
            string body = "A new appointment request has been made. Check your profile";

            await _emailService.SendEmailAsync(caregiverEmail, subject, body);

            return RedirectToAction("Profile", new { id = appointment.CaregiverId });
        }
    }
}
