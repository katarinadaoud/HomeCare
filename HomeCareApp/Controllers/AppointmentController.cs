using HomeCareApp.DAL;
using HomeCareApp.Models;
using HomeCareApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims; // added for notificcation
using Microsoft.EntityFrameworkCore;  // ADD – for FirstOrDefaultAsync()
using System.Linq;  // ADD – for Where() and Select() 
using Microsoft.Extensions.Logging; // ADD – for logging    

namespace HomeCareApp.Controllers
{ // Controller for managing appointments

    [Authorize]

    public class AppointmentController : Controller
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly ILogger<AppointmentController> _logger; // Logger for logging information and errors
        private readonly AppDbContext _db; // ADD
        public AppointmentController(
            
            IAppointmentRepository appointmentRepository,
            AppDbContext db,               // ADD
            ILogger<AppointmentController> logger)
        {

            _appointmentRepository = appointmentRepository;
            _db = db;                     // ADD
            _logger = logger;
           

        }

        public async Task<IActionResult> Table() // Lists all appointments in a table view
        {
            ViewBag.Role = "patient";
            ViewBag.ActiveTab = "appointments";
            
            var appointments = await _appointmentRepository.GetAll();
            if (appointments == null)
            {
                _logger.LogError("[AppointmentController] Appointment list not found while executing _appointmentRepository.GetAll()");
                return NotFound("Appointment list not found");
            }
            var appointmentsViewModel = new AppointmentViewModel(appointments, "Table"); // Using the same ViewModel for different views
            return View(appointmentsViewModel);
        }

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

            if (ModelState.IsValid) // Validates the model state
            {
                _logger.LogWarning("[AppointmentController] invalid model {@appointment}", appointment);
                return View(appointment);
            }

            // Bind appointment til innlogget pasient (hvis mulig)
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
            if (!created) // If creation failed
            {
                _logger.LogWarning("[AppointmentController] appointment creation failed {@appointment}", appointment);
                return View(appointment);
            }

            // Opprett Notification hvis pasient ble funnet
            if (appointment.PatientId > 0)
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


            // If model state is invalid or creation failed
            _logger.LogWarning("[AppointmentController] appointment creation failed {@appointment}", appointment);
            return View(appointment);
        }
        

        public IActionResult Confirmation()
        {
            ViewBag.Role = "patient";
            ViewBag.ActiveTab = "appointments";
            return View();
        }

        [HttpGet]
        
 // Endpoint to fetch appointments in FullCalendar format
[HttpGet]
public async Task<IActionResult> Events()
        {
            ViewBag.Role = "patient";
            ViewBag.ActiveTab = "appointments";
    try
    {
                var appointments = await _appointmentRepository.GetAll();

        // Endpoint to fetch appointments in FullCalendar format
        [HttpGet]
        public async Task<IActionResult> Events()
        {
            try
            {
                var appointments = await _appointmentRepository.GetAll();

                // Converts appointments to FullCalendar format

                var events = appointments.Select(a => new
                {
                    id = a.AppointmentId,
                    title = string.IsNullOrWhiteSpace(a.Subject)
                        ? "Appointment"
                        : a.Subject,
                    description = a.Description,
                    start = a.Date.ToString("yyyy-MM-dd"), // FullCalendar  requires date in ISO format
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


        [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
            ViewBag.Role = "patient";
            ViewBag.ActiveTab = "appointments";
        
        var appointment = await _appointmentRepository.GetAppointmentById(id);
        if (appointment == null)
        {
            var appointment = await _appointmentRepository.GetAppointmentById(id);
            if (appointment == null)
            {
                _logger.LogError("[AppointmentController] Appointment not found when updating the ItemId {ItemId:0000}", id);
                return BadRequest("Appointment not found for the ItemId");
            }
            return View(appointment);
        }

    [HttpPost]
    public async Task<IActionResult> Update(Appointment appointment)
        {
            ViewBag.Role = "patient";
            ViewBag.ActiveTab = "appointments";

        if (ModelState.IsValid)
        {
            if (ModelState.IsValid)
            {
                bool returnOk = await _appointmentRepository.Update(appointment);
                if (returnOk)
                    return RedirectToAction(nameof(Table));
            }
            _logger.LogWarning("[AppointmentController] Appointment update failed {@item}", appointment);
            return View(appointment);
        }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
        {
            ViewBag.Role = "patient";
            ViewBag.ActiveTab = "appointments";

        var appointment = await _appointmentRepository.GetAppointmentById(id);
        if (appointment == null)
        {
            var appointment = await _appointmentRepository.GetAppointmentById(id);
            if (appointment == null)
            {
                _logger.LogError("[AppointmentController] Appointment not found for the AppointmentId {AppointmentId:0000}", id);
                return BadRequest("Appointment not found for the AppointmentId");
            }
            return View(appointment);
        }

    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ViewBag.Role = "patient";
            ViewBag.ActiveTab = "appointments";

        bool returnOk = await _appointmentRepository.Delete(id);
        if (!returnOk)
        {
            bool returnOk = await _appointmentRepository.Delete(id);
            if (!returnOk)
            {
                _logger.LogError("[AppointmentController] Appointment deletion failed for the AppointmentId {AppointmentId:0000}", id);
                return BadRequest("Item deletion failed");
            }
            return RedirectToAction(nameof(Table));
        }

    }
    
    
}
