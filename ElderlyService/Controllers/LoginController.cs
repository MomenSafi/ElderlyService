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

            if (!isCaregiver)
            {
                user.RoleId = "3";
                user.userId = Guid.NewGuid().ToString();
                _db.Users.Add(user);
                _db.SaveChanges();
            }
            else
            {
                user.RoleId = "2";
                user.userId = Guid.NewGuid().ToString();
                user.IsCaregiver = true;
                _db.Users.Add(user);
                _db.SaveChanges();

                var cargiver = new Caregiver();
                cargiver.CaregiverId = Guid.NewGuid().ToString();
                cargiver.userId = user.userId;
                _db.Caregivers.Add(cargiver);
                _db.SaveChanges();
            }

            return RedirectToAction("MyProfile", "User");
        }

        public IActionResult Forgotpassword()
        {
            return RedirectToAction("LogIn");
        }
        public IActionResult RegisrationCaregiver()
        {
            return View();
        }
    }
}
