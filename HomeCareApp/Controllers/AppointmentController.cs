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
    public async Task<IActionResult> Book()
    {
        var availableDays = await _appointmentRepository.GetAvailableDays();
        return View(new AppointmentBookViewModel
        {
            Appointment = new Appointment(),
            AvailableDays = availableDays
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Book(Appointment appointment)
    {
        if (!ModelState.IsValid)
        {
            var vm = new AppointmentBookViewModel
            {
                Appointment = appointment,
                AvailableDays = await _appointmentRepository.GetAvailableDays()
            };
            return View(vm);
        }

        await _appointmentRepository.Create(appointment);
        TempData["Success"] = "Success!";
        return RedirectToAction(nameof(Confirmation));
    }


    public IActionResult Confirmation()
    {
        return View(); // Confirmation.cshtml
    }
}