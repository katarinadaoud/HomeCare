using Microsoft.AspNetCore.Mvc;
using HomeCareApp.Models;
using HomeCareApp.ViewModels;

namespace HomeCareApp.Controllers;

public class EmployeeController : Controller
{
    private readonly AppDbContext _appDbContext;

    public EmployeeController(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public IActionResult Table()
    {
        List<Employee> employees = _appDbContext.Employees.ToList();
        var employeesViewModel = new EmployeesViewModel(employees, "Table");
        return View(employeesViewModel);
    }

    public IActionResult Grid()
    {
        List<Employee> employees = _appDbContext.Employees.ToList();
        var employeesViewModel = new EmployeesViewModel(employees, "Table");
        return View(employeesViewModel);
    }
    
    public IActionResult Details(int id)
    {
        List<Employee> employees = _appDbContext.Employees.ToList();
        var employee = employees.FirstOrDefault(i => i.Employee_id == id);
        if (employee == null)
            return NotFound();
        return View(employee);
    }
}