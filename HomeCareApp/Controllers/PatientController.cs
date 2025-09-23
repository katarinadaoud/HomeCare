using Microsoft.AspNetCore.Mvc;
using HomeCareApp.Models;
using HomeCareApp.ViewModels;

namespace HomeCareApp.Controllers;

public class PatientController : Controller
{
    private readonly AppDbContext _appDbContext;

    public PatientController(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

     public IActionResult Patient()
    {
        return View();
    }

    /*

    public IActionResult Table()
    {
        List<Patient> patients = _appDbContext.Patients.ToList();
        var patientsViewModel = new PatientsViewModel(patients, "Table");
        return View(patientsViewModel);
    }

    public IActionResult Grid()
    {
        List<Patient> patients = _appDbContext.Patients.ToList();
        var patientsViewModel = new PatientsViewModel(patients, "Grid");
        return View(patientsViewModel);
    }
    
    public IActionResult Details(int id)
    {
        List<Patient> patients = _appDbContext.Patients.ToList();
        var patient = patients.FirstOrDefault(i => i.Patient_id == id);
        if (patient == null)
            return NotFound();
        return View(patient);
    } */
}