using ElderlyService.Data;
using ElderlyService.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;

namespace ElderlyService.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _db;
        public LoginController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
        }
        [HttpPost]
        [ActionName("LogIn")]
        public IActionResult LogIn_post([Bind("Email", "Password")] Users user)
        {
            var users = _db.Users.ToList();
            foreach (var u in users)
            {
                if (u.Email.ToLower() == user.Email.ToLower())
                {
                    if (u.Password == user.Password)
                    {
                        string userJson = JsonConvert.SerializeObject(u);
                        HttpContext.Session.SetString("LiveUser", userJson);
                        if (u.RoleId == "1")
                            return RedirectToAction("Dashboard", "Admin");

                        return RedirectToAction("Index", "User");
                    }
                    else
                    {
                        TempData["error"] = "Wrong password";
                        return View();
                    }
                }
            }

            TempData["info"] = "This Email isn't registered, please SignUp";
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("LiveUser");
            return RedirectToAction("Index", "User");
        }
        public IActionResult Regesration(Users user, bool isCaregiver)
        {
            var existingUser = _db.Users.FirstOrDefault(u => u.Email.ToLower() == user.Email.ToLower());

            if (existingUser != null)
            {
                TempData["error"] = "This Email is registered, please Login";
                return RedirectToAction("LogIn");
            }

            string userJson = JsonConvert.SerializeObject(user);
            HttpContext.Session.SetString("LiveUser", userJson);

            // Set common properties
            user.RoleId = isCaregiver ? "2" : "3";
            user.userId = Guid.NewGuid().ToString();

            // Add user to the database
            _db.Users.Add(user);
            _db.SaveChanges();

            // Update the session with the modified user object
            HttpContext.Session.SetString("LiveUser", JsonConvert.SerializeObject(user));

            if (isCaregiver)
            {
                var caregiver = new Caregiver
                {
                    CaregiverId = Guid.NewGuid().ToString(),
                    userId = user.userId
                };

                // Add caregiver to the database
                _db.Caregivers.Add(caregiver);
                _db.SaveChanges();
            }

            return RedirectToAction("MyProfile", "User");
        }



        public IActionResult Forgotpassword()
        {
            return RedirectToAction("LogIn");
        }
    }
}
