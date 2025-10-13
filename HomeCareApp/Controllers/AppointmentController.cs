using Microsoft.AspNetCore.Mvc;
using HomeCareApp.Models;
using HomeCareApp.ViewModels;
using HomeCareApp.DAL;
using System.Drawing.Printing;

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
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Book(Appointment appointment)
    {
        if (!ModelState.IsValid)
        {
            await _appointmentRepository.Create(appointment);
            return RedirectToAction(nameof(Confirmation));
        }
        return BadRequest("Did not succeed");
    }

    public IActionResult Confirmation()
    {
        return View(); // Confirmation.cshtml
    }
}