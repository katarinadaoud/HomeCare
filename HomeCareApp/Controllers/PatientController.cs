using Microsoft.AspNetCore.Mvc;
using HomeCareApp.Models;
using HomeCareApp.ViewModels;
using HomeCareApp.DAL;

namespace HomeCareApp.Controllers;

public class PatientController : Controller
{
    private readonly IPatientRepository _patientRepository;

    public PatientController(IPatientRepository patientRepository)
    {
        _patientRepository = patientRepository;
    }

     public async Task<IActionResult> Index()
    {
        var patients = await _patientRepository.GetAll();
        ViewBag.CurrentViewName = "Patients List";
        return View(patients);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Patient patient)
    {
        if (ModelState.IsValid)
        {
            await _patientRepository.Create(patient);
            return RedirectToAction(nameof(Index));
        }
        return View(patient);
    }

}