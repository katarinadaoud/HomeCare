using Microsoft.AspNetCore.Mvc;
using HomeCareApp.Models;
using HomeCareApp.DAL;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace HomeCareApp.Controllers;

[Authorize]
public class PatientController : Controller
{
    private readonly IPatientRepository _patientRepository;

    public PatientController(IPatientRepository patientRepository)
    {
        _patientRepository = patientRepository;
    }
//Patient dashboard
    public IActionResult Index()
    {
        ViewBag.Role = "patient";
        ViewBag.ActiveTab = "appointments";
        return View(); 
    }


    //CRUD operations for patients
    [HttpGet]
    public IActionResult CreatePatient()
    {
        ViewBag.Role = "employee";
        ViewBag.ActiveTab = "patients";
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreatePatient(HomeCareApp.Models.Patient patient)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        ModelState.Remove(nameof(patient.UserId));
        if (!string.IsNullOrEmpty(userId)) patient.UserId = userId;
        TryValidateModel(patient);
        {
            if (!string.IsNullOrEmpty(userId))
                patient.UserId = userId;
        }

        if (!ModelState.IsValid)
        {
            ViewBag.Role = "employee";
            ViewBag.ActiveTab = "patients";
            return View(patient);
        }

        await _patientRepository.Create(patient);
        return RedirectToAction("Table", "Employee"); // endre fra "Patients" til "Table"
    }    

    [HttpGet]
    public async Task<IActionResult> UpdatePatient(int id)
    {
        var patient = await _patientRepository.GetPatientById(id);
        if (patient == null) return NotFound();

        ViewBag.Role = "employee";
        ViewBag.ActiveTab = "patients"; 
        return View(patient);
    }
    [HttpPost]
    public async Task<IActionResult> UpdatePatient(Patient patient)
    {
        if (ModelState.IsValid)
        {
            await _patientRepository.Update(patient);
            return RedirectToAction("Table", "Employee"); 
        }
        ViewBag.Role = "employee";
        ViewBag.ActiveTab = "patients";
        return View(patient);
    }

    [HttpGet]
    public async Task<IActionResult> DeletePatient(int id)
    {
        var patient = await _patientRepository.GetPatientById(id);
        if (patient == null) return NotFound();

        ViewBag.Role = "employee";      
        ViewBag.ActiveTab = "patients";
        return View(patient);
    }

        [HttpPost, ActionName("DeletePatient")] 
        public async Task<IActionResult> DeletePatientConfirmed(int id)
        {
            await _patientRepository.Delete(id);
            return RedirectToAction("Table", "Employee");
        }
    }
