using HomeCareApp.DAL;
using HomeCareApp.Models;
using HomeCareApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims; // for notifications
using Microsoft.EntityFrameworkCore;  // for FirstOrDefaultAsync()
using System.Linq;  // for Where() and Select() 
using Microsoft.Extensions.Logging; // for logging    

namespace HomeCareApp.Controllers
{
    // Controller for managing appointments
    [Authorize]
    public class AppointmentController : Controller
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly ILogger<AppointmentController> _logger;
        private readonly AppDbContext _db;

        public AppointmentController(
            IAppointmentRepository appointmentRepository,
            AppDbContext db,
            ILogger<AppointmentController> logger)
        {
            _appointmentRepository = appointmentRepository;
            _db = db;
            _logger = logger;
        }

        // Lists all appointments in a table view
        public async Task<IActionResult> Table()
        {
            ViewBag.Role = "patient";
            ViewBag.ActiveTab = "appointments";

            var appointments = await _appointmentRepository.GetAll();
            if (appointments == null)
            {
                _logger.LogError("[AppointmentController] Appointment list not found while executing _appointmentRepository.GetAll()");
                return NotFound("Appointment list not found");
            }

            var vm = new AppointmentViewModel(appointments, "Table");
            return View(vm);
        }

        // ----- Book -----
        [HttpGet]
        public IActionResult Book()
        {
            ViewBag.Role = "patient";
            ViewBag.ActiveTab = "appointments";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Book(Appointment appointment)
        {
            ViewBag.Role = "patient";
            ViewBag.ActiveTab = "appointments";

            // KORRIGERT: returner view hvis modellen er UGYLDIG
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("[AppointmentController] invalid model {@appointment}", appointment);
                return View(appointment);
            }

            // Knytt time til innlogget pasient (hvis mulig)
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(userId))
            {
                var pid = await _db.Patients
                    .Where(p => p.UserId == userId)
                    .Select(p => (int?)p.PatientId)
                    .FirstOrDefaultAsync();

                if (pid.HasValue) appointment.PatientId = pid.Value;
            }

            var created = await _appointmentRepository.Create(appointment);
            if (!created)
            {
                _logger.LogWarning("[AppointmentController] appointment creation failed {@appointment}", appointment);
                return View(appointment);
            }

            // Opprett Notification hvis pasientId finnes
            if (appointment.PatientId.HasValue && appointment.PatientId.Value > 0)
            {
                _db.Notifications.Add(new Notification
                {
                    PatientId = appointment.PatientId.Value,
                    Message = $"Time bestilt: {appointment.Date:dd.MM.yyyy} – {(appointment.Subject ?? string.Empty)}",
                    CreatedAt = DateTime.UtcNow,
                    IsRead = false
                });
                await _db.SaveChangesAsync();
            }

            TempData["Success"] = "Appointment booked successfully!";
            return RedirectToAction(nameof(Confirmation));
        }

        public IActionResult Confirmation()
        {
            ViewBag.Role = "patient";
            ViewBag.ActiveTab = "appointments";
            return View();
        }

        // ----- Calendar Events (FullCalendar) -----
        // Single, clean endpoint (duplikat fjernet)
        [HttpGet]
        public async Task<IActionResult> Events()
        {
            ViewBag.Role = "patient";
            ViewBag.ActiveTab = "appointments";

            try
            {
                var appointments = await _appointmentRepository.GetAll();
                var events = appointments.Select(a => new
                {
                    id = a.AppointmentId,
                    title = string.IsNullOrWhiteSpace(a.Subject) ? "Appointment" : a.Subject,
                    description = a.Description,
                    start = a.Date.ToString("yyyy-MM-dd"),
                    end = a.Date.ToString("yyyy-MM-dd"),
                    allDay = true
                });

                return Json(events);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[AppointmentController] Error while fetching events");
                return StatusCode(500, new { message = "Failed to load calendar events." });
            }
        }

        // ----- Update -----
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            ViewBag.Role = "patient";
            ViewBag.ActiveTab = "appointments";

            var appointment = await _appointmentRepository.GetAppointmentById(id);
            if (appointment == null)
            {
                _logger.LogError("[AppointmentController] Appointment not found when updating the AppointmentId {AppointmentId:0000}", id);
                return BadRequest("Appointment not found for the AppointmentId");
            }
            return View(appointment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Appointment appointment)
        {
            ViewBag.Role = "patient";
            ViewBag.ActiveTab = "appointments";

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("[AppointmentController] Invalid model on update {@appointment}", appointment);
                return View(appointment);
            }

            bool ok = await _appointmentRepository.Update(appointment);
            if (ok) return RedirectToAction(nameof(Table));

            _logger.LogWarning("[AppointmentController] Appointment update failed {@appointment}", appointment);
            return View(appointment);
        }

        // ----- Delete -----
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            ViewBag.Role = "patient";
            ViewBag.ActiveTab = "appointments";

            var appointment = await _appointmentRepository.GetAppointmentById(id);
            if (appointment == null)
            {
                _logger.LogError("[AppointmentController] Appointment not found for the AppointmentId {AppointmentId:0000}", id);
                return BadRequest("Appointment not found for the AppointmentId");
            }
            return View(appointment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ViewBag.Role = "patient";
            ViewBag.ActiveTab = "appointments";

            bool ok = await _appointmentRepository.Delete(id);
            if (!ok)
            {
                _logger.LogError("[AppointmentController] Appointment deletion failed for the AppointmentId {AppointmentId:0000}", id);
                return BadRequest("Appointment deletion failed");
            }
            return RedirectToAction(nameof(Table));
        }
    }
}
