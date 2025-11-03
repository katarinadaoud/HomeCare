using Microsoft.AspNetCore.Mvc;
using HomeCareApp.Models;
using HomeCareApp.DAL;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using HomeCareApp.Controllers;
using System.Linq; 
namespace HomeCareApp.Controllers;

[Authorize]
public class PatientController : Controller
{
    private readonly IPatientRepository _patientRepository;
    private readonly AppDbContext _db; // ADD

    public PatientController(IPatientRepository patientRepository, AppDbContext db)

    {
    _patientRepository = patientRepository;
    _db = db ?? throw new ArgumentNullException(nameof(db));
    }


//Patient dashboard
    public IActionResult Index()
    {
        ViewBag.Role = "patient";
        ViewBag.ActiveTab = "appointments";
        // ADD: gi JS tilgang til pasientens id (for bjella)
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!string.IsNullOrEmpty(userId))
        {
        ViewBag.PatientId = _db.Patients
        .Where(p => p.UserId == userId)
        .Select(p => (int?)p.PatientId)
        .FirstOrDefault();
        }

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
