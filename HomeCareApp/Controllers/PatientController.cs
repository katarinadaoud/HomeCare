using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HomeCareApp.Models;
using HomeCareApp.DAL;
using System.Security.Claims;

namespace HomeCareApp.Controllers;

[Authorize]
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
[ValidateAntiForgeryToken]
public async Task<IActionResult> Create(Patient patient)
{
    Console.WriteLine(">>> HIT Create(POST)");

    // Sett UserId fra innlogget bruker (og fjern ev. ModelState-feil på UserId)
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    ModelState.Remove(nameof(Patient.UserId));
    if (!string.IsNullOrEmpty(userId))
        patient.UserId = userId;

    // Valider på nytt etter at UserId er satt
    TryValidateModel(patient);

    if (!ModelState.IsValid)
    {
        Console.WriteLine(">>> MODELSTATE INVALID");
        foreach (var kv in ModelState)
            foreach (var err in kv.Value.Errors)
                Console.WriteLine($">>> MODEL ERROR {kv.Key}: {err.ErrorMessage}");

        ViewBag.Role = "patient";
        ViewBag.ActiveTab = "patients";
        return View(patient);
    }

    Console.WriteLine(">>> MODELSTATE VALID -> saving…");
    await _patientRepository.Create(patient);
    Console.WriteLine(">>> SAVED (Create) ✔");

    return RedirectToAction(nameof(Patients));
}

    [HttpGet]
    public async Task<IActionResult> UpdatePatient(int id)
    {
        // Gets the patient by id
        var patient = await _patientRepository.GetItemById(id);
        if (patient == null)
        {
            return View("UpdatePatient", patient);
        }
        ViewBag.Role = "patient";
        ViewBag.ActiveTab = "patients";
        return View(patient);
    }
    // Update Patient POST
    [HttpPost]
    [ValidateAntiForgeryToken] // Protects against CSRF attacks
    public async Task<IActionResult> Update(Patient patient)
    {
        //UserId is auto assigned to the logged in user, without this line ModelState will be invalid
        if (User.Identity?.IsAuthenticated == true && string.IsNullOrWhiteSpace(patient.UserId))
            patient.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

         if (!ModelState.IsValid)
    {
        ViewBag.Role = "patient";
        ViewBag.ActiveTab = "patients";
        return View("UpdatePatient", patient);
    }

    
        await _patientRepository.Update(patient);
        return RedirectToAction(nameof(Patients));
        

    }

    // Delete Patient
    [HttpPost]
    [ValidateAntiForgeryToken] // Protects against CSRF attacks
    public async Task<IActionResult> Delete(int id)
    {
        await _patientRepository.Delete(id);
        return RedirectToAction(nameof(Patients));
    }

}

