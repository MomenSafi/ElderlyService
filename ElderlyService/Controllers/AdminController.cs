using ElderlyService.Data;
using ElderlyService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ElderlyService.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment webHostEnvironment;
        public AdminController (ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            this.webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
         ///////////////////////////////////////////////////////////
         // Service

        public IActionResult Service()
        {
            var service = _db.services.ToList();
            return View(service);
        }

        public IActionResult AddService()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddService(Service service)
        {
            _db.services.Add(service);
            _db.SaveChanges();
            TempData["success"] = "Service Added Successfully";
            return RedirectToAction("Service");
        }

        public IActionResult EditService(int? id)
        {
            var service = _db.services.Find(id);
            return View(service);
        }
        [HttpPost]
        public IActionResult EditService(Service service)
        {
            _db.services.Update(service);
            _db.SaveChanges();
            TempData["success"] = "Service Updated Successfully";
            return RedirectToAction("Service");
        }
        ////////////////////////////////////////
        // users
        public IActionResult Users()
        {
            var users = _db.Users
                .Where(u=>u.RoleId == "3")
                .ToList();
            return View(users);
        }

        /// //////////////////////////////////
        // caregiver

        public IActionResult Caregiver()
        {
            var Caregivers = _db.Caregivers
                .Include(c=>c.Users)
                .Where(c=>c.Users.RoleId == "2")
                .ToList();
            return View(Caregivers);
        }
        //////////////////////////////////////////
        // Review
        public IActionResult Review() 
        {
            var reviews = _db.Reviews
                .Include(r=>r.Users)
                .Include(r=>r.Caregiver)
                .Where(r=>r.status == Reviews.Status.pending)
                .ToList();
            return View();
        }
        public IActionResult ShowOrDelReview(int? id, bool status)
        {
            var review = _db.Reviews.Find(id);
            if (status == true)
            {
                review.status = Reviews.Status.Approved;
                TempData["success"] = "Review Added to Elderly Successfully";
            }
            else
            {
                review.status = Reviews.Status.Rejected;
                TempData["success"] = "Review deleted to Elderly Successfully";
            }
            return View();
        }
        //////////////////////////////////////////
        // Testimonials
        public IActionResult Testimonials()
        {
            var reviews = _db.reviewsForWebsites
                .Include(r => r.Users)
                .Where(r => r.status == ReviewsForWebsites.Status.pending)
                .ToList();
            return View();
        }
        public IActionResult ShowOrDelTestimonials(int? id, bool status)
        {
            var review = _db.reviewsForWebsites.Find(id);
            if (status == true)
            {
                review.status = ReviewsForWebsites.Status.Approved;
                TempData["success"] = "Testimonials Added to Elderly Successfully";
            }
            else
            {
                review.status = ReviewsForWebsites.Status.Rejected;
                TempData["success"] = "Testimonials deleted to Elderly Successfully";
            }
            return View();
        }
    }
}
