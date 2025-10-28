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

    // ✅ Always return a non-null list (fix for NullReferenceException)
    public async Task<IEnumerable<Medication>> GetAll()
    {
        try
        {
            var meds = await _db.Medications
                .Include(m => m.Patient)
                .AsNoTracking()
                .ToListAsync();

            return meds ?? new List<Medication>(); // never return null
        }
        catch (Exception e)
        {
            _logger.LogError("[MedicationRepository] GetAll() failed: {e}", e.Message);
            return new List<Medication>(); // safe fallback
        }
    }

    // ✅ Get single medication by ID with patient info
    public async Task<Medication?> GetMedById(int id)
    {
        try
        {
            return await _db.Medications
                .Include(m => m.Patient)
                .FirstOrDefaultAsync(m => m.MedicationId == id);
        }
        catch (Exception e)
        {
            _logger.LogError("[MedicationRepository] GetMedById({id}) failed: {e}", id, e.Message);
            return null;
        }
    }

    // ✅ Create a new medication and link it to the correct patient
    public async Task<bool> Create(Medication medication)
    {
        try
        {
            if (medication.PatientId == null)
            {
                _logger.LogWarning("[MedicationRepository] Tried to create medication without PatientId");
                return false;
            }

            // Create a stub patient object (bypasses 'required' validation)
            var stubPatient = (Patient)Activator.CreateInstance(typeof(Patient))!;
            stubPatient.PatientId = medication.PatientId.Value;
            // Attach the existing patient reference without loading full data
            _db.Attach(stubPatient).State = EntityState.Unchanged;

            _db.Medications.Add(medication);
            await _db.SaveChangesAsync();

            _logger.LogInformation("[MedicationRepository] Successfully created medication {Name}", medication.Name);
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[MedicationRepository] Create() failed: {e}", e.Message);
            return false;
        }
    }

    // ✅ Update an existing medication
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
            _logger.LogError("[MedicationRepository] Update() failed for ID {id}: {e}", medication.MedicationId, e.Message);
            return false;
        }
    }

    // ✅ Delete a medication by ID
    public async Task<bool> Delete(int id)
    {
        try
        {
            var medication = await _db.Medications.FindAsync(id);
            if (medication == null)
            {
                _logger.LogWarning("[MedicationRepository] Delete() failed — medication not found for ID {id}", id);
                return false;
            }

            _db.Medications.Remove(medication);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[MedicationRepository] Delete() failed for ID {id}: {e}", id, e.Message);
            return false;
        }
    }
}
