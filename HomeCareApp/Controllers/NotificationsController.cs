//using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HomeCareApp.DAL;
using HomeCareApp.Models;
using System.Linq;
using System.Threading.Tasks;

namespace HomeCareApp.Controllers;

//[Authorize] // Sikrer at kun autentiserte brukere kan få tilgang til disse endepunktene
[Route("Notifications")]
public class NotificationsController : Controller
{
    private readonly AppDbContext _db;
    public NotificationsController(AppDbContext db) => _db = db;

    // Amount of unread notifications 
    [HttpGet("UnreadCount")]
    public async Task<IActionResult> UnreadCount(int patientId) /*PatientId må kanskje byttes ut senere når booking osv er på plass*/
    {
        var count = await _db.Notifications

            //filtrerer på pasient
            .Where(n => n.PatientId == patientId && !n.IsRead)
            .CountAsync(); // teller antall uleste

        return Json(count); // returnerer antall uleste, brukes i JS på bjelle
    }

    // Henter de siste 5 notifikasjonene
    [HttpGet("Latest")]
    public async Task<IActionResult> Latest(int patientId, int take = 10) /*Take sier hvor mange varslinger man henter som standard, kanskje lavere enn 10 for ytelse?*/
    {
        var items = await _db.Notifications
            .Where(n => n.PatientId == patientId)
            .OrderByDescending(n => n.CreatedAt) /*Nyeste først*/
            .Take(take)
            .Select(n => new { n.NotificationId, n.Message, n.CreatedAt, n.IsRead }) 
            .ToListAsync();

        return Json(items); // returnerer liste med notifikasjoner, om vi vil ha dropdown
    }

    // Marker en notifikasjon som lest
    [HttpPost("MarkRead")]
    public async Task<IActionResult> MarkRead(int id)
    {
        var n = await _db.Notifications.FindAsync(id); /*Find henter varselet fra db*/
        if (n == null) return NotFound(); /*returnerer 404 hvis ikke funnet*/

        n.IsRead = true; // markerer som lest
        await _db.SaveChangesAsync(); /*Lagrer endringen i db*/
        return Ok(); // returnerer 200 OK
    }
}


