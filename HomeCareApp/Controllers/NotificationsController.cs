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
        if (!ModelState.IsValid) return View(model);
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

        return Json(count);
    }

        [HttpGet("Latest")]
        public async Task<IActionResult> Latest(int patientId, int take = 10)
        {
            var items = await _db.Notifications
                .Where(n => n.PatientId == patientId)
                .OrderByDescending(n => n.CreatedAt)
                .Take(Math.Clamp(take, 1, 20)) // litt vern
                .Select(n => new { n.NotificationId, n.Message, n.CreatedAt, n.IsRead })
                .ToListAsync();

            return Json(items);
        }

         [HttpPost("MarkRead")]
        public async Task<IActionResult> MarkRead(int id)
        {
            var n = await _db.Notifications.FindAsync(id);
            if (n == null) return NotFound();

            n.IsRead = true;
            await _db.SaveChangesAsync();
            return Ok();
        }
    }





    /* Henter de siste 5 notifikasjonene
    [HttpGet("Latest")]
    public async Task<IActionResult> Latest(int patientId, int take = 10) Take sier hvor mange varslinger man henter som standard, kanskje lavere enn 10 for ytelse?
    {
        var items = await _db.Notifications
            .Where(n => n.PatientId == patientId)
            .OrderByDescending(n => n.CreatedAt) /*Nyeste først
            .Take(take)
            .Select(n => new { n.NotificationId, n.Message, n.CreatedAt, n.IsRead }) 
            .ToListAsync();

        return Json(items); // returnerer liste med notifikasjoner, om vi vil ha dropdown
    }

    // Marker en notifikasjon som lest
    [HttpPost("MarkRead")]
    public async Task<IActionResult> MarkRead(int id)
    {
        var n = await _db.Notifications.FindAsync(id); /*Find henter varselet fra db
        if (n == null) return NotFound(); /*returnerer 404 hvis ikke funnet

        n.IsRead = true; // markerer som lest
        await _db.SaveChangesAsync(); /*Lagrer endringen i db
        return Ok(); // returnerer 200 OK
    } */



