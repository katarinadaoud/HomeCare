namespace HomeCareApp.Models;

// ViewModel for handling error information
public class ErrorViewModel
{
    public string? RequestId { get; set; }
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}
