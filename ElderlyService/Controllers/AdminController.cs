using ElderlyService.Data;
using ElderlyService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
        public IActionResult Dashboard()
        {
            ViewBag.ProfessionalCaregiver = _db.Caregivers.Count();
            ViewBag.UserUseWebsite = _db.Users.Count(r => r.RoleId == "3");
            ViewBag.AppointmentApproved = _db.Appointments.Count(a => a.status == Models.Appointment.Status.Approved);
            ViewBag.paymentsApprovd = _db.Payments
                .Where(p=>p.status == Models.Payment.Status.Approved)
                .Sum(p=>p.Amount);

            var appointment = _db.Appointments
                .Include(a => a.Users)
                .Include(a => a.Caregiver)
                    .ThenInclude(c => c.Users)
                .ToList();
            return View(appointment);
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
            if (service.ImageFile != null)
            {
                string wwwRootPath = webHostEnvironment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + "" + service.ImageFile.FileName;
                string path = Path.Combine(wwwRootPath + "/Image/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    service.ImageFile.CopyTo(fileStream);
                }
                service.ImageUrl = "/Image/" + fileName;
            }
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
            if (service.ImageFile != null)
            {
                string wwwRootPath = webHostEnvironment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + "" + service.ImageFile.FileName;
                string path = Path.Combine(wwwRootPath + "/Image/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    service.ImageFile.CopyTo(fileStream);
                }
                service.ImageUrl = "/Image/" + fileName;
            }
            _db.services.Update(service);
            _db.SaveChanges();
            TempData["success"] = "Service Updated Successfully";
            return RedirectToAction("Service");
        }
        [HttpPost]
        public IActionResult Service(string SearchItem)
        {
            if (!string.IsNullOrEmpty(SearchItem))
            {
                List<Service> Search = _db.services.Where(u => u.Name.Contains(SearchItem)).ToList();
                return View(Search);
            }
            var service = _db.services.ToList();
            return View(service);
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
        [HttpPost]
        public IActionResult Users(string SearchItem)
        {
            if(string.IsNullOrEmpty(SearchItem))
            {
                List<Users> Search = _db.Users.Where(u=>u.FirstName.Contains(SearchItem) || u.LastName.Contains(SearchItem)).ToList();
                return View(Search);
            }
            var users = _db.Users
                .Where(u => u.RoleId == "3")
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
                .Include(c=>c.Service)
                .ToList();
            return View(Caregivers);
        }
        [HttpPost]
        public IActionResult Caregiver(string SearchItem)
        {
            if (string.IsNullOrEmpty(SearchItem))
            {
                List<Caregiver> Search = _db.Caregivers.Where(c => c.Users.FirstName.Contains(SearchItem) || c.Users.LastName.Contains(SearchItem))
                    .Include(c=>c.Users)
                    .Where(c=>c.Users.RoleId =="2")
                    .Include (c=>c.Service)
                    .ToList();
                return View(Search);
            }
            var Caregivers = _db.Caregivers
                .Include(c => c.Users)
                .Where(c => c.Users.RoleId == "2")
                .Include(c => c.Service)
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
                    .ThenInclude(r=>r.Users)
                .ToList();
            return View(reviews);
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
            _db.Reviews.Update(review);
            _db.SaveChanges();
            return RedirectToAction("Review");
        }
        //////////////////////////////////////////
        // Testimonials
        public IActionResult Testimonials()
        {
            var reviews = _db.reviewsForWebsites
                .Include(r => r.Users)
                .ToList();
            return View(reviews);
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
            _db.SaveChanges();
            return RedirectToAction("Testimonials");
        }

        public IActionResult Payment()
        {
            var payments = _db.Payments
                .Include(p=>p.Caregiver)
                .ThenInclude(c=>c.Users)
                .ToList();
            return View(payments);
        }

        public IActionResult ActiveOrRejectCaregiver(int? id, bool status)
        {
            var payment = _db.Payments.Find(id);
            if(status)
            {
                payment.status = Models.Payment.Status.Approved;
                var caregiver = _db.Caregivers.SingleOrDefault(c => c.CaregiverId == payment.CaregiverId);
                if(payment.Amount == 10)
                {
                    if (caregiver.Valid == false)
                    {
                        caregiver.EndSubscribe = DateTime.Now.AddMonths(1);
                    }
                    else
                    {
                        caregiver.EndSubscribe = caregiver.EndSubscribe.Value.AddMonths(1);
                    }
                }
                else if(payment.Amount == 20)
                {
                    if (caregiver.Valid == false)
                    {
                        caregiver.EndSubscribe = DateTime.Now.AddMonths(2);
                    }
                    else 
                    {
                        caregiver.EndSubscribe = caregiver.EndSubscribe.Value.AddMonths(2);
                    }
                }
                else
                {
                    if (caregiver.Valid == false)
                    {
                        caregiver.EndSubscribe = DateTime.Now.AddMonths(3);
                    }
                    else
                    {
                        caregiver.EndSubscribe = caregiver.EndSubscribe.Value.AddMonths(3);
                    }
                }
                caregiver.Valid = true;
                TempData["success"] = "payment accepted";
                _db.Payments.Update(payment);
                _db.Caregivers.Update(caregiver);
                _db.SaveChanges();
                return RedirectToAction("Payment");
            }
            else
            {
                payment.status = Models.Payment.Status.Rejected;
                _db.SaveChanges();
                TempData["success"] = "Payment rejected";
                return RedirectToAction("Payment");
            }
        }
    }
}
