using HomeCareApp.Models;

namespace HomeCareApp.ViewModels;

public class AppointmentBookViewModel
{
    public Appointment Appointment { get; set; }
    public IEnumerable<DateTime> AvailableDays { get; set; }
}