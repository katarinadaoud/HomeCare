// Controllers/AppointmentController.cs
using System.Linq;
using System.Threading.Tasks;
using HomeCareApp.DAL;
using HomeCareApp.Models;
using HomeCareApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

namespace HomeCareApp.Controllers
{
    // [Authorize]  // valgfritt å slå på når du vil
    public class AppointmentController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<AppointmentController> _logger;

        public AppointmentController(
            AppDbContext db,
            IAppointmentRepository appointmentRepository,
            UserManager<User> userManager,
            ILogger<AppointmentController> logger)
        {
            _db = db;
            _appointmentRepository = appointmentRepository;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Book()
        {
            var vm = new AppointmentBookViewModel
            {
                Appointment = new Appointment
                {
                    Date = System.DateTime.Today.AddDays(1) // kan overstyres av kalenderen/hidden field
                },
                PatientOptions = await BuildPatientOptionsAsync()
            };

            _logger.LogInformation("Book GET: Loaded {Count} patients for selection.",
                vm.PatientOptions?.Count() ?? 0);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Book(AppointmentBookViewModel model)
        {
            // 1) innlogget bruker?
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
            {
                _logger.LogWarning("Book POST: No logged-in user. Challenging.");
                return Challenge();
            }

            // 2) slå opp EmployeeId for denne brukeren (MÅ settes for å unngå FK-feil)
            var employeeId = await _db.Employees
                .Where(e => e.UserId == user.Id)
                .Select(e => e.EmployeeId)
                .SingleOrDefaultAsync();

            if (employeeId == 0)
            {
                _logger.LogWarning("Book POST: No Employee row found for user {UserId}", user.Id);
                ModelState.AddModelError(string.Empty, "Ingen ansattprofil knyttet til innlogget bruker.");
            }
            else
            {
                model.Appointment.EmployeeId = employeeId; // ← NØDVENDIG
            }

            // 3) valider at valgt pasient finnes
            var patientOk = await _db.Patients
                .AnyAsync(p => p.PatientId == model.Appointment.PatientId);

            if (!patientOk)
            {
                _logger.LogWarning("Book POST: Invalid PatientId={PatientId}", model.Appointment.PatientId);
                ModelState.AddModelError(nameof(Appointment.PatientId), "Ugyldig pasient valgt.");
            }

            if (!ModelState.IsValid)
            {
                model.PatientOptions = await BuildPatientOptionsAsync();
                return View(model);
            }

            // 4) lagre
            await _appointmentRepository.Create(model.Appointment);

            TempData["Success"] = "Appointment booked successfully.";
            return RedirectToAction(nameof(Confirmation));
        }

        public IActionResult Confirmation() => View();

        // === Lite JSON-endepunkt for kalenderen (serverer lagrede avtaler) ===
        // GET /Appointment/Events?from=2025-10-01&to=2025-10-31
        [HttpGet("/Appointment/Events")]
        public async Task<IActionResult> Events(DateTime? from, DateTime? to)
        {
            var q = _db.Appointments.AsQueryable();
            if (from.HasValue) q = q.Where(a => a.Date >= from.Value.Date);
            if (to.HasValue)   q = q.Where(a => a.Date <= to.Value.Date);

            var data = await q
                .Select(a => new
                {
                    id    = a.AppointmentId,
                    title = a.Subject,
                    start = a.Date.ToString("yyyy-MM-dd")
                })
                .ToListAsync();

            return Ok(data);
        }

        private async Task<SelectList> BuildPatientOptionsAsync()
        {
            var patients = await _db.Patients
                .Select(p => new { p.PatientId, p.FullName })
                .ToListAsync();

            return new SelectList(patients, "PatientId", "FullName");
        }
    }
}
