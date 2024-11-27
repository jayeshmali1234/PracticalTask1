using PracticalTask1.DbContext; // Import your DbContext namespace
using PracticalTask1.Models; // Import the SubCategoryModel view model namespace
using System.Linq;
using System.Web.Mvc;

namespace PracticalTask1.Controllers
{
    public class UserController : Controller
    {
        private readonly PracticalInterviewDBEntities _context;

        // Constructor to initialize the DbContext
        public UserController()
        {
            _context = new PracticalInterviewDBEntities();  // Initialize the DbContext
        }

        public ActionResult Dashboard()
        {
            // Assuming we are fetching categories assigned to the current user
            int currentUserId = 1;  // For example, assuming user with ID 1 is logged in

            // Fetch the categories assigned to the user
            var userCategories = _context.UserCategories
                                         .Where(uc => uc.UserId == currentUserId)
                                         .Select(uc => uc.Category)
                                         .ToList();

            // Map the categories and their subcategories to the view model
            var subCategories = userCategories.SelectMany(c => c.SubCategories)
                                              .Select(sc => new Models.SubCategory
                                              {
                                                  Id = sc.Id,
                                                  Name = sc.Name
                                              }).ToList();

            return View(subCategories);  // Pass the list of subcategories to the view
        }
    }
}
