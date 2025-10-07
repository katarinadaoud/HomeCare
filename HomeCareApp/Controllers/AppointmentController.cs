using Microsoft.AspNetCore.Mvc;
using HomeCareApp.Models;
using HomeCareApp.ViewModels;
using HomeCareApp.DAL;

namespace HomeCareApp.Controllers;

public class AppointmentController : Controller
{
    private readonly IAppointmentRepository _appointmentRepository;

    public AppointmentController(IAppointmentRepository appointmentRepository)
    {
        _appointmentRepository = appointmentRepository;
    }

    [HttpGet]
    public IActionResult Book()
    {
        return View(); // Book.cshtml
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Book(Appointment appointment)
    {
        if (!ModelState.IsValid)
            return View(appointment);

        await _appointmentRepository.Create(appointment);
        TempData["Success"] = "Timebestilling sendt!";
        return RedirectToAction("Confirmation");
    }

    public IActionResult Confirmation()
    {
        return View(); // Confirmation.cshtml
    }
}