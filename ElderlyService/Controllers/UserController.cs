using ElderlyService.Data;
using ElderlyService.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq;

namespace ElderlyService.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;
        public UserController(ApplicationDbContext db)
        {
            _db = db;
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
                ReviewsForWebsitess = _db.reviewsForWebsites.Where(s => s.status == ReviewsForWebsites.Status.Approved).ToList()
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
            ViewBag.Services = _db.services.ToList();
            caregiver = _db.Caregivers
                .Include(c => c.Users)
                .Include(c=>c.Reviews)
                .Where(c=>c.Valid == true).ToList();
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
                    .Where(c => c.AvilableForThisWeek != null && c.AvilableForThisWeek.Any(a => selectedDayOfWeek.Contains(a.DayOfWeek.ToString())))
                    .ToList();
            }

            if (!string.IsNullOrEmpty(startTime) && !string.IsNullOrEmpty(endTime))
            {
                DateTime start = DateTime.Today.Add(TimeSpan.Parse(startTime));
                DateTime end = DateTime.Today.Add(TimeSpan.Parse(endTime));

                caregiver = caregiver
                    .Where(c => c.AvilableForThisWeek.Any(a => a.StartTime.TimeOfDay >= start.TimeOfDay && a.EndTime.TimeOfDay <= end.TimeOfDay))
                    .ToList();
            }

            return View(caregiver);
        }

        public IActionResult Testimonials()
        {
            var testimonials = _db.reviewsForWebsites.ToList();
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
                .Where(c=>c.Valid == true)
                .Include(c => c.Reviews) // Include Reviews navigation property
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

        public IActionResult NotiNotification()
        {
            return View();
        }


        public IActionResult Profile(string id)
        {
            return View();
        }

        public IActionResult MyProfile()
        {
            return View();
        }
    }
}
