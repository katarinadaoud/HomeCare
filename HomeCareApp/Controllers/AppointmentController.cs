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

namespace HomeCareApp.Controllers
{
    [Authorize] // valgfritt, men anbefalt
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

        // GET: /Appointment/Book
        [HttpGet]
        public async Task<IActionResult> Book()
        {
            var vm = new AppointmentBookViewModel
            {
                Appointment = new Appointment
                {
                    // Forhåndssett dato (kalenderen kan overstyre via hidden field)
                    Date = System.DateTime.Today.AddDays(1)
                },
                PatientOptions = await BuildPatientOptionsAsync()
            };

            // Hjelpelogg: hvor mange pasienter finnes i dropdown?
            _logger.LogInformation("Book GET: Loaded {Count} patients for selection.",
                vm.PatientOptions?.Count() ?? 0);

            return View(vm);
        }

        // POST: /Appointment/Book
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Book(AppointmentBookViewModel model)
        {
            // 1) Sjekk innlogget bruker
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
            {
                _logger.LogWarning("Book POST: No logged-in user. Challenging.");
                return Challenge();
            }

            // 2) Slå opp EmployeeId for innlogget bruker
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
                model.Appointment.EmployeeId = employeeId;
            }

            // Hjelpelogg: hva prøver vi å lagre?
            _logger.LogInformation("Book POST incoming: PatientId={PatientId}, EmployeeId={EmployeeId}, Date={Date}",
                model.Appointment.PatientId, model.Appointment.EmployeeId, model.Appointment.Date);

            // 3) Valider at valgt pasient finnes (hindrer FK-feil)
            var patientOk = await _db.Patients
                .AnyAsync(p => p.PatientId == model.Appointment.PatientId);

            if (!patientOk)
            {
                _logger.LogWarning("Book POST: Invalid PatientId={PatientId}", model.Appointment.PatientId);
                ModelState.AddModelError(nameof(Appointment.PatientId), "Ugyldig pasient valgt.");
            }

            // 4) Hvis validering feiler: repopuler dropdown og returner view
            if (!ModelState.IsValid)
            {
                model.PatientOptions = await BuildPatientOptionsAsync();
                return View(model);
            }

            // 5) Lagre (kun FK-verdier; ikke attach nav-objekter)
            await _appointmentRepository.Create(model.Appointment);

            TempData["Success"] = "Appointment booked successfully.";
            return RedirectToAction(nameof(Confirmation));
        }

        public IActionResult Confirmation()
        {
            return View(); // bruker ditt eksisterende Confirmation-view
        }

        // Hjelper: bygg pasient-listen for dropdown
        private async Task<SelectList> BuildPatientOptionsAsync()
        {
            var patients = await _db.Patients
                .Select(p => new { p.PatientId, p.FullName })
                .ToListAsync();

            return new SelectList(patients, "PatientId", "FullName");
        }
    }
}
