using Microsoft.EntityFrameworkCore;
using HomeCareApp.Models;

namespace HomeCareApp.DAL;

public class MedicationRepository : IMedicationRepository
{
    private readonly AppDbContext _db;

    private readonly ILogger<MedicationRepository> _logger;

    public MedicationRepository(AppDbContext db, ILogger<MedicationRepository> logger)
    {
        _db = db;
        _logger = logger;
    }

    // Get all medications with patient information
    public async Task<IEnumerable<Medication>?> GetAll()
    {
        try
        {
            return await _db.Medications
                .Include(m => m.Patient) // ← LEGG TIL DENNE LINJEN
                .AsNoTracking()
                .ToListAsync();
        }
        catch (Exception e)
        {
            _logger.LogError("[MedicationRepository] medications ToListAsync() failed when GetAll(), error message: {e}", e.Message);
            return null;
        }
    }

    // Get medication by ID with patient information
    public async Task<Medication?> GetMedById(int id)
    {
        try
        {
            return await _db.Medications
                .Include(m => m.Patient) // ← LEGG TIL DENNE LINJEN
                .FirstOrDefaultAsync(m => m.MedicationId == id);
        }
        catch (Exception e)
        {
            _logger.LogError("[MedicationRepository] medication FindAsync(id) failed when GetMedById for MedicationId {MedicationId:0000}, error message: {e}", id, e.Message);
            return null;
        }
    }

    public async Task<bool> Create(Medication medication)
    {
        try
        {
            Console.WriteLine("=== Repository.Create() called ===");
            
            // TEMP: Fjern foreign key for testing
            medication.PatientId = null;
            medication.Patient = null;
            
            Console.WriteLine($"Creating: {medication.Name}");
            
            _db.Medications.Add(medication);
            await _db.SaveChangesAsync();
            
            Console.WriteLine("SUCCESS!");
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine($"DETAILED ERROR: {e.Message}");
            Console.WriteLine($"Inner: {e.InnerException?.Message}");
            return false;
        }
    }

    public async Task<bool> Update(Medication medication)
    {
        try
        {
            _db.Medications.Update(medication);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[MedicationRepository] medication FindAsync(id) failed when updating the MedicationId {MedicationId:0000}, error message: {e}", medication.MedicationId, e.Message);
            return false;
        }
    }

    public async Task<bool> Delete(int id)
    {
        try
        {
            var medication = await _db.Medications.FindAsync(id);
            if (medication == null)
            {
                _logger.LogError("[MedicationRepository] medication not found for the MedicationId {MedicationId:0000}", id);
                return false;
            }

            _db.Medications.Remove(medication);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[MedicationRepository] medication deletion failed for the MedicationId {MedicationId:0000}, error message: {e}", id, e.Message);
            return false;
        }
    }
}
