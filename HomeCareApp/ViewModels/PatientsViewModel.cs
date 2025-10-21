using HomeCareApp.Models;

// ViewModel for managing patients
namespace HomeCareApp.ViewModels
{
    public class PatientsViewModel
    { 
        public IEnumerable<Patient> Patients;
        public string? CurrentViewName;

        public PatientsViewModel(IEnumerable<Patient> patients, string? currentViewName)
        {
            Patients = patients;
            CurrentViewName = currentViewName;
        }
    }
}