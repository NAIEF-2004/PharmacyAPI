using System.Collections.Generic;
using System.Threading.Tasks;
using PharmacyAPI.Models;

namespace PharmacyAPI.Repository
{
    public interface IPrescriptionRepository
    {
        Task<IEnumerable<Prescription>> GetAllAsync();
        Task<Prescription> GetByIdAsync(int id);
        Task AddAsync(Prescription prescription);
        Task UpdateAsync(Prescription prescription);
        Task DeleteAsync(int id);
    }
}
