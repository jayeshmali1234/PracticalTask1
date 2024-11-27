using PracticalTask1.DbContext;
using PracticalTask1.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace PracticalTask1.Controllers
{
    public class AdminController : Controller
    {
        private readonly PracticalInterviewDBEntities _context; // Use your DbContext name

        public AdminController()
        {
            _context = new PracticalInterviewDBEntities(); // Initialize DbContext
        }

        // GET: Admin (List all users)
        public ActionResult Index()
        {
            var users = _context.Users.ToList(); // Fetch all users
            using (var context = new PracticalInterviewDBEntities())
            {
                var userModels = users.Select(u => new UserModel
                {
                    Id = u.Id,
                    Name = u.Name,
                    Phone = u.Phone,
                    Email = u.Email,
                    Password = u.Password,
                    IsAdmin = u.IsAdmin
                }).ToList();
                return View(userModels); // Pass the list of users to the view
            }
        }

        // GET: Admin/Create (Show the form to create a new user)
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create (Save new user to the database)
        [HttpPost]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                // Ensure the password is set before saving
                if (string.IsNullOrEmpty(user.Password))
                {
                    user.Password = "DefaultPassword123"; // Or generate a secure password
                }
                user.Password = HashPassword(user.Password);
                _context.Users.Add(user); // Add user to DbContext

                try
                {
                    _context.SaveChanges(); // Save changes to the database
                    return RedirectToAction("Dashboard", "User", new { userId = user.Id });
                }
                catch (DbEntityValidationException ex)
                {
                    // Log validation errors
                    foreach (var validationError in ex.EntityValidationErrors)
                    {
                        foreach (var error in validationError.ValidationErrors)
                        {
                            ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                        }
                    }
                    return RedirectToAction("Index");
                }
            }
            return View(user);
        }

        public ActionResult Edit(int id)
        {
            var user = _context.Users.Find(id); // Find user by ID
            if (user == null)
            {
                return HttpNotFound();
            }
            var model = new UserModel
            {
                Id = user.Id,
                Name = user.Name,
                Phone = user.Phone,
                Email = user.Email,
                Password= user.Password,
                IsAdmin = user.IsAdmin
            };

            return View(model);
        }

        // POST: Admin/Edit/5 (Update user details)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.Find(model.Id); // Fetch the user from the database
                if (user == null)
                    return HttpNotFound();

                // Update user details
                user.Name = model.Name;
                user.Phone = model.Phone;
                user.Email = model.Email;
                user.IsAdmin = model.IsAdmin;

                // Update the password only if a new one is provided
                if (!string.IsNullOrEmpty(model.Password))
                {
                    user.Password = HashPassword(model.Password); // Hash the password if provided
                }

                _context.SaveChanges(); // Save changes to the database
                return RedirectToAction("Index");
            }

            return View(model);
        }

        public ActionResult Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            var model = new UserModel
            {
                Id = user.Id,
                Name = user.Name,
                Phone = user.Phone,
                Email = user.Email,
                IsAdmin = user.IsAdmin
            };

            return View(model);
        }

        // POST: Admin/Delete/5 (Delete user from database)
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }
    }
}