//using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HomeCareApp.DAL;
using HomeCareApp.Models;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace HomeCareApp.Controllers;

//[Authorize] // Only authenticated users can access
[Route("Notifications")]
[AutoValidateAntiforgeryToken]

public class NotificationsController : Controller
{
    private readonly AppDbContext _db;
    public NotificationsController(AppDbContext db) => _db = db;

    // Amount of unread notifications 
     [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var items = await _db.Notifications // makes a query to the Notifications table
                .OrderByDescending(n => n.IsRead) // filter to get unread notifications first
                .Take(100) // limit to 100 notifications
                .ToListAsync();

            return View(items);
        }

    [HttpGet("Create")]
    public IActionResult Create() => View(new Notification());

    [HttpPost("Create")]
    public async Task<IActionResult> Create(Notification model)
    {
        if (!ModelState.IsValid) return View(model); // return view with validation errors
        model.CreatedAt = DateTime.UtcNow;

        _db.Notifications.Add(model);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    //API for notifications

    [HttpGet("UnreadCount")]
    public async Task<IActionResult> UnreadCount(int patientId)
    {
        var count = await _db.Notifications
            .Where(n => n.PatientId == patientId && !n.IsRead)
            .CountAsync();

        return Json(count); // return count as JSON
    }

        [HttpGet("Latest")]
        public async Task<IActionResult> Latest(int patientId, int take = 10)
        {
            var items = await _db.Notifications
                .Where(n => n.PatientId == patientId)
                .OrderByDescending(n => n.CreatedAt) // latest first
                .Take(Math.Clamp(take, 1, 20)) // limit between 1 and 20
                .Select(n => new { n.NotificationId, n.Message, n.CreatedAt, n.IsRead }) // select only necessary fields
                .ToListAsync();

            return Json(items);
        }

    [HttpPost("MarkRead")]
    public async Task<IActionResult> MarkRead(int id) // Marks a notification as read
    {
        var n = await _db.Notifications.FindAsync(id); // Find notification by ID
        if (n == null) return NotFound();

        n.IsRead = true; // Mark as read
        await _db.SaveChangesAsync();
        return Ok();
    }
        
        [HttpPost("MarkAllRead")]
public async Task<IActionResult> MarkAllRead(int patientId)
{
    var items = await _db.Notifications
        .Where(n => n.PatientId == patientId && !n.IsRead)
        .ToListAsync();

    if (items.Count == 0) return Ok();

    foreach (var n in items) n.IsRead = true;
    await _db.SaveChangesAsync();
    return Ok();
}

}
