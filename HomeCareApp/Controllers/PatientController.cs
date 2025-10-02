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
    public async Task<IActionResult> Create()
    {
        var patients = await _patientRepository.GetAll();
        return View(patients);
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

    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        var patient = await _patientRepository.GetItemById(id);
        if (patient == null)
        {
            return NotFound();
        }
        return View(patient);
    }

    [HttpPost]
    public async Task<IActionResult> Update(Patient patient)
    {
        if (ModelState.IsValid)
        {
            await _patientRepository.Update(patient);
            return RedirectToAction(nameof(Patient));
        }

        return View(patient);
    }
    
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var patient = await _patientRepository.GetItemById(id);
        if (patient == null)
        {
            return NotFound();
        }
        return View(patient);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _patientRepository.Delete(id);
        return RedirectToAction(nameof(Patient));
    }
}