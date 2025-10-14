using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HomeCareApp.Models;
using HomeCareApp.ViewModels;
using HomeCareApp.DAL;
using System.Threading.Tasks;

namespace HomeCareApp.Controllers;

[Authorize] // Sikrer at kun autentiserte brukere kan få tilgang til disse endepunktene
public class EmployeeController : Controller
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeController(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    // CHANGE: /Employee -> redirect til kalender
    public IActionResult Index()
    {
        return RedirectToAction(nameof(Schedule)); // ←
    }

    // NEW: egen action for listevisning (tidl. i Index)
    public async Task<IActionResult> EmployeesList()
    {
        var employees = await _employeeRepository.GetAll();
        ViewBag.CurrentViewName = "Employees List";
        ViewBag.Role = "employee";
        ViewBag.ActiveTab = "patients";
        return View("Employee", employees);
    }

    // Kalenderdashboard for ansatte (viser Views/Employee/Index.cshtml)
    public IActionResult Schedule()
    {
        ViewBag.Role = "employee";       // sørger for riktig topnav/branding
        ViewData["Role"] = "employee";   // ekstra sikkerhet for layout
        ViewBag.ActiveTab = "schedule";
        return View("Index");            // viser Views/Employee/Index.cshtml
    }

    // CHANGE: behold alias som tidligere ble brukt, men redirect nå til lista
    public IActionResult Employee() => RedirectToAction(nameof(EmployeesList)); // ←

    [HttpGet]
    public IActionResult CreateEmployee()
    {
        ViewBag.Role = "employee";
        ViewBag.ActiveTab = "patients";
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateEmployee(Employee employee)
    {
        if (ModelState.IsValid)
        {
            await _employeeRepository.Create(employee);
            return RedirectToAction(nameof(EmployeesList)); // ← CHANGE
        }
        ViewBag.Role = "employee";
        ViewBag.ActiveTab = "patients";
        return View(employee);
    }

    [HttpGet]
    public async Task<IActionResult> UpdateEmployee(int id)
    {
        var employee = await _employeeRepository.GetItemById(id);
        if (employee == null) return NotFound();

        ViewBag.Role = "employee";      // NEW
        ViewBag.ActiveTab = "patients"; // NEW
        return View(employee);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateEmployee(Employee employee)
    {
        if (ModelState.IsValid)
        {
            await _employeeRepository.Update(employee);
            return RedirectToAction(nameof(EmployeesList)); // ← CHANGE
        }
        ViewBag.Role = "employee";
        ViewBag.ActiveTab = "patients";
        return View(employee);
    }

    [HttpGet]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        var employee = await _employeeRepository.GetItemById(id);
        if (employee == null) return NotFound();

        ViewBag.Role = "employee";      // NEW
        ViewBag.ActiveTab = "patients"; // NEW
        return View(employee);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteEmployeeConfirmed(int id)
    {
        await _employeeRepository.Delete(id);
        return RedirectToAction(nameof(EmployeesList)); // ← CHANGE
    }

    // NEW: stub for "Today’s visits" slik at TopNav-lenken ikke 404'er
    [HttpGet]
    public IActionResult Visits() // ← NEW
    {
        ViewBag.Role = "employee";      // NEW
        ViewBag.ActiveTab = "visits";   // NEW
        return View();                  // forventer Views/Employee/Visits.cshtml (kan være en enkel placeholder)
    }
}
