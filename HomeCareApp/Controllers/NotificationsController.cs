using HomeCareApp.DAL;
using HomeCareApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace HomeCareApp.Controllers
{
    public class NotificationsController : Controller
    {
        private readonly AppDbContext _db;
        public NotificationsController(AppDbContext db) => _db = db;

        // GET: /Notifications
        public IActionResult Index() //list all notifications
        {
            var list = _db.Notifications // Include the Patient details in the notification list
                          .OrderByDescending(n => n.CreatedAt) // show newest first
                          .ToList();
            return View(list);
        }

        [HttpGet]
        public IActionResult Create() //show form
        {
            return View(); //view with empty form
        }
        

        // POST: /Notifications/Create

        [HttpPost] // Handle form submission
        [ValidateAntiForgeryToken] //protect against forged requests
        public IActionResult Create(Notification n)
        {
            if (!ModelState.IsValid) return View(n); //if validation fails, return to the form with the entered data
            n.CreatedAt = DateTime.UtcNow; // Set the creation time to the current UTC time
            n.IsRead = false; // New notifications are unread by default
            _db.Notifications.Add(n);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
