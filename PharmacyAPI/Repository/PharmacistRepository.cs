using Microsoft.EntityFrameworkCore;
using PharmacyAPI.Data;
using PharmacyAPI.Models;
using static PharmacyAPI.Data.PharmacyDbContext;

namespace PharmacyAPI.Repository
{
    public class PharmacistRepository : IPharmacistRepository
    {
        private readonly PharmacyDbContext _context;

        public PharmacistRepository(PharmacyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Pharmacist>> GetAllAsync()
        {
            return await _context.Pharmacists.ToListAsync();
        }

        public async Task<Pharmacist> GetByIdAsync(int id)
        {
            return await _context.Pharmacists.FindAsync(id);
        }

        public async Task AddAsync(Pharmacist pharmacist)
        {
            await _context.Pharmacists.AddAsync(pharmacist);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Pharmacist pharmacist)
        {
            _context.Pharmacists.Update(pharmacist);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var pharmacist = await _context.Pharmacists.FindAsync(id);
            if (pharmacist != null)
            {
                _context.Pharmacists.Remove(pharmacist);
                await _context.SaveChangesAsync();
            }
        }
    }
}
