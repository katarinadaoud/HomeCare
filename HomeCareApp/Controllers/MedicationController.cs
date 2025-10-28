using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HomeCareApp.DAL;
using HomeCareApp.Models;
using HomeCareApp.ViewModels;

namespace HomeCareApp.Controllers;

public class MedicationController : Controller
{
    private readonly IMedicationRepository _medicationRepository;
    private readonly ILogger<MedicationController> _logger;
    private readonly IPatientRepository _patientRepository; // Legg til dette feltet

    public MedicationController(IMedicationRepository medicationRepository, ILogger<MedicationController> logger, IPatientRepository patientRepository)
    {
        _medicationRepository = medicationRepository;
        _logger = logger;
        _patientRepository = patientRepository; // Initialiser feltet
    }

    public async Task<IActionResult> Table()
    {
        var items = await _medicationRepository.GetAll();
        if (items == null)
        {
            _logger.LogError("[MedicationController] Medication list not found while executing _medicationRepository.GetAll()");
            return NotFound("Medication list not found");
        }
        var medsViewModel = new MedsViewModel(items, "Table");
        return View(medsViewModel);
    }

    [HttpGet]
    public async Task<IActionResult> CreateMed()
    {
        ViewBag.Role = "employee";
        ViewBag.ActiveTab = "medications";

        // Hent alle pasienter for dropdown
        var patients = await _patientRepository.GetAll();
        ViewBag.Patients = patients;

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateMed(Medication medication)
    {
        Console.WriteLine("=== CreateMed POST called ===");
        Console.WriteLine($"Name: {medication.Name}");
        Console.WriteLine($"PatientId: {medication.PatientId}");
        Console.WriteLine($"Dosage: {medication.Dosage}");

        // Sjekk ModelState
        Console.WriteLine($"ModelState.IsValid: {ModelState.IsValid}");

        if (!ModelState.IsValid)
        {
            Console.WriteLine("VALIDATION ERRORS:");
            foreach (var error in ModelState)
            {
                foreach (var err in error.Value.Errors)
                {
                    Console.WriteLine($"- {error.Key}: {err.ErrorMessage}");
                }
            }

            // Reload patients
            var patients = await _patientRepository.GetAll();
            ViewBag.Patients = patients;
            ViewBag.Role = "employee";
            ViewBag.ActiveTab = "medications";
            return View(medication);
        }

        Console.WriteLine("Calling repository.Create()...");
        var success = await _medicationRepository.Create(medication);
        Console.WriteLine($"Repository.Create() result: {success}");

        if (success)
        {
            Console.WriteLine("Redirecting to Table...");
            return RedirectToAction("Table");
        }
        else
        {
            Console.WriteLine("Create failed, returning view...");
            ModelState.AddModelError("", "Failed to save medication");

            // Reload patients
            var patients = await _patientRepository.GetAll();
            ViewBag.Patients = patients;
            ViewBag.Role = "employee";
            ViewBag.ActiveTab = "medications";
            return View(medication);
        }
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> UpdateMed(int id)
    {
        var medication = await _medicationRepository.GetMedById(id);
        if (medication == null)
        {
            _logger.LogError("[MedicationController] Medication not found when updating the MedicationId {MedicationId:0000}", id);
            return BadRequest("Medication not found for the MedicationId");
        }
        return View(medication);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> UpdateMed(Medication medication)
    {
        if (ModelState.IsValid)
        {
            bool returnOk = await _medicationRepository.Update(medication);
            if (returnOk)
                return RedirectToAction(nameof(Table));
        }
        _logger.LogWarning("[MedicationController] Medication update failed {@medication}", medication);
        return View(medication);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> DeleteMed(int id)
    {
        var medication = await _medicationRepository.GetMedById(id);
        if (medication == null)
        {
            _logger.LogError("[MedicationController] Medication not found for the MedicationId {MedicationId:0000}", id);
            return BadRequest("Medication not found for the MedicationId");
        }
        return View(medication);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> DeleteMedConfirmed(int id)
    {
        bool returnOk = await _medicationRepository.Delete(id);
        if (!returnOk)
        {
            _logger.LogError("[MedicationController] Medication deletion failed for the MedicationId {MedicationId:0000}", id);
            return BadRequest("Medication deletion failed");
        }
        return RedirectToAction(nameof(Table));
    }

}