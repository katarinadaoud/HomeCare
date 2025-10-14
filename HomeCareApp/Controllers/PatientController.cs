using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HomeCareApp.Models;
using HomeCareApp.ViewModels;
using HomeCareApp.DAL;
using System.Security.Cryptography.X509Certificates;
using NuGet.Protocol.Core.Types;

namespace HomeCareApp.Controllers;

[Authorize] // Sikrer at kun autentiserte brukere kan få tilgang til disse endepunktene
public class PatientController : Controller
{
    private readonly IPatientRepository _patientRepository;

    public PatientController(IPatientRepository patientRepository)
    {
        _patientRepository = patientRepository;
    }

    // CHANGED: Index er nå pasientportalen (kalender-UI), ingen modell lastes her
    public IActionResult Index()
    {
        ViewBag.Role = "patient";               // NEW: sørger for pasient-topnav + SOS i layout
        ViewBag.ActiveTab = "appointments";     // NEW: marker riktig fane i pasient-UI
        return View();                          // viser Views/Patient/Index.cshtml
    }

    // NEW: egen action for liste over pasienter (tidligere lå i Index)
    public async Task<IActionResult> Patients()
    {
        var patients = await _patientRepository.GetAll();
        ViewBag.CurrentViewName = "Patients List";
        ViewBag.Role = "employee";                 // NEW: endre til "patient" hvis lista skal vises i pasientflaten
        ViewBag.ActiveTab = "patients";         // NEW: valgfritt, for å markere fane i staff-topnav
        return View("Patient", patients);
    }

    [HttpGet]
    public IActionResult Create()
    {
        ViewBag.Role = "patient";            // Endret til "patient" hvis dette er pasientflaten
        ViewBag.ActiveTab = "patients";
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken] // Protects against CSRF attacks
    public async Task<IActionResult> Create(Patient patient)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Role = "patient";
            ViewBag.ActiveTab = "patients";
            return View(patient);
        }

        await _patientRepository.Create(patient);
        return RedirectToAction(nameof(Patients));
    }
}

