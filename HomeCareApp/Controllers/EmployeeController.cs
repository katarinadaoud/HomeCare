using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HomeCareApp.DAL;
using HomeCareApp.Models;
using HomeCareApp.ViewModels; // Husk å legge til denne using-setningen

namespace HomeCareApp.Controllers;

[Authorize]
public class EmployeeController : Controller
{
    private readonly IPatientRepository _patientRepository;

    public EmployeeController(IPatientRepository patientRepository)
    {
        _patientRepository = patientRepository;
    }

    // Employee dashboard
    public IActionResult Index()
    {
        ViewBag.Role = "employee";
        ViewBag.ActiveTab = "schedule";
        return View(); // viser Views/Employee/Index.cshtml
    }

    // Patients list
    public async Task<IActionResult> Table()
    {
        var patients = await _patientRepository.GetAll();
        ViewBag.Role = "employee";
        ViewBag.ActiveTab = "patients";

        var viewModel = new PatientsViewModel(patients, "Table"); // ← Bruk constructor
        return View(viewModel); // viser Views/Employee/Table.cshtml automatisk
    }

    // Today's visits
    public IActionResult Visits()
    {
        ViewBag.Role = "employee";
        ViewBag.ActiveTab = "visits";
        return View();
    }
}