using HomeCareApp.DAL;
using HomeCareApp.Models;
using HomeCareApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace HomeCareApp.Controllers
{
    [Authorize]
    public class AppointmentController : Controller
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly ILogger<AppointmentController> _logger;

        public AppointmentController(
            IAppointmentRepository appointmentRepository,
            ILogger<AppointmentController> logger)
        {

            _appointmentRepository = appointmentRepository;
            _logger = logger;
        }

        public async Task<IActionResult> Table()
        {
            var appointments = await _appointmentRepository.GetAll();
            if (appointments == null)
            {
                _logger.LogError("[AppointmentController] Appointment list not found while executing _appointmentRepository.GetAll()");
                return NotFound("Appointment list not found");
            }
            var appointmentsViewModel = new AppointmentViewModel(appointments, "Table");
            return View(appointmentsViewModel);
        }

        [HttpGet]
        public IActionResult Book()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Book(Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                bool returnOk = await _appointmentRepository.Create(appointment);
                if (returnOk)
                {
                    TempData["Success"] = "Appointment booked successfully!";
                    return RedirectToAction(nameof(Confirmation));
                }

            }
            _logger.LogWarning("[AppointmentController] appointment creation failed {@appointment}", appointment);
            return View(appointment);
        }
        
        public IActionResult Confirmation()
        {
            return View();
        }

        [HttpGet]
    public async Task<IActionResult> Update(int id)
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
