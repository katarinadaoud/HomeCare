//using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HomeCareApp.Models;
using HomeCareApp.ViewModels;
using HomeCareApp.DAL;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Castle.Components.DictionaryAdapter.Xml;
using System.Security.Claims;


namespace HomeCareApp.Controllers;

//[Authorize] // Makes sure only authorized users can access the controller
public class EmployeeController : Controller
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeController(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    // CHANGE: /Employee -> redirect to schedule
    public IActionResult Index()
    {
        return RedirectToAction(nameof(Schedule)); // ←
    }

    // This method returns a view displaying the list of employees

    public async Task<IActionResult> EmployeesList()
    {
        var employees = await _employeeRepository.GetAll();
        ViewBag.CurrentViewName = "Employees List";
        ViewBag.Role = "employee";
        ViewBag.ActiveTab = "patients";
        return View("Employee", employees);
    }


    // this makes sure that when navigating to /Employee/Schedule, the correct view is shown
    public IActionResult Schedule()
    {
        ViewBag.Role = "employee";       //makes sure layout knows the role
        ViewBag.ActiveTab = "schedule";  //makes sure layout knows the active tab
        ViewData["Role"] = "employee";
        ViewBag.ActiveTab = "schedule";
        return View("Index");            // shows Views/Employee/Index.cshtml
    }

    // redirect /Employee/Employee to /Employee/EmployeesList
    public IActionResult Employee() => RedirectToAction(nameof(EmployeesList)); // ←

    [HttpGet]
    public IActionResult CreateEmployee()
    {
        ViewBag.Role = "employee";
        ViewBag.ActiveTab = "patients";
        return View();
    }

    [HttpPost]

    public async Task<IActionResult> CreateEmployee(HomeCareApp.Models.Employee employee)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); //Sets UserId from logged in user

        ModelState.Remove(nameof(employee.UserId));
        if (!string.IsNullOrEmpty(userId)) employee.UserId = userId;
        TryValidateModel(employee); // validate the model again after setting UserId
        {
            employee.UserId = userId;
        }


        if (ModelState.IsValid)
        {
            ViewBag.Role = "employee";
            ViewBag.ActiveTab = "patients";
            return View(employee);

        }
        await _employeeRepository.Create(employee);
        return RedirectToAction(nameof(EmployeesList));
    }





    /* Må nulle dette
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
    } */

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

    /*POST delete*/
    [HttpPost, ActionName("DeleteEmployee")] // specifies that this action handles POST requests for DeleteEmployee
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteEmployeeConfirmed(int id)
    {
        await _employeeRepository.Delete(id);
        return RedirectToAction(nameof(EmployeesList)); // ← CHANGE
    }


    [HttpGet]
    public IActionResult Visits()
    {
        ViewBag.Role = "employee";
        ViewBag.ActiveTab = "visits";
        return View();

    }
}
//Slettes ettersom det er validering i CreateEmployee

    /*Validation
        [HttpPost]

        public async Task<IActionResult> Create(Employee employee)
        {
        if (!ModelState.IsValid)
        {
            ViewBag.Role = "employee";
            ViewBag.ActiveTab = "patients";
            return View("employee", employee);
        }
    
            await _employeeRepository.Create(employee);
            return RedirectToAction(nameof(employee));
        }
    } */ 
    
