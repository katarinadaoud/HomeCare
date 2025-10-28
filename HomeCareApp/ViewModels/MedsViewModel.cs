using HomeCareApp.Models;

// ViewModel for managing medications
namespace HomeCareApp.ViewModels
{
    public class MedsViewModel
    {
        public IEnumerable<Medication> Medications { get; set; } = new List<Medication>();
        public string? CurrentViewName { get; set; }

        // Legg til parameterløs constructor
        public MedsViewModel() { }

        // Legg til constructor med parametere
        public MedsViewModel(IEnumerable<Medication> medications, string? currentViewName)
        {
            Medications = medications;
            CurrentViewName = currentViewName;
        }
    }
}