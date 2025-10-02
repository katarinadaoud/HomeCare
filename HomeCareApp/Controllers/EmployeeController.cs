using Microsoft.AspNetCore.Mvc;
using HomeCareApp.Models;
using HomeCareApp.ViewModels;
using HomeCareApp.DAL;
using System.Threading.Tasks;

namespace HomeCareApp.Controllers;

public class EmployeeController : Controller
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeController(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<IActionResult> Index()
    {
        var employees = await _employeeRepository.GetAll();
        ViewBag.CurrentViewName = "Employees List";
        return View("Employee", employees);
    }

    [HttpGet]
    public IActionResult CreateEmployee()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateEmployee(Employee employee)
    {
        if (ModelState.IsValid)
        {
            await _employeeRepository.Create(employee);
            return RedirectToAction(nameof(Employee));
        }
        return View(employee);
    }

    [HttpGet]
    public async Task<IActionResult> UpdateEmployee(int id)
    {
        var employee = await _employeeRepository.GetItemById(id);
        if (employee == null)
        {
            return NotFound();
        }
        return View(employee);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateEmployee(Employee employee)
    {
        if (ModelState.IsValid)
        {
            await _employeeRepository.Update(employee);
            return RedirectToAction(nameof(Employee));
        }

        return View(employee);
    }
    
    [HttpGet]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        var employee = await _employeeRepository.GetItemById(id);
        if (employee == null)
        {
            return NotFound();
        }
        return View(employee);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteEmployeeConfirmed(int id)
    {
        await _employeeRepository.Delete(id);
        return RedirectToAction(nameof(Employee));
    }

    
}