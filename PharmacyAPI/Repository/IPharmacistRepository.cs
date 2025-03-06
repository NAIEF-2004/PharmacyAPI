using PharmacyAPI.Models;

namespace PharmacyAPI.Repository
{
    public interface IPharmacistRepository
    {
        Task<IEnumerable<Pharmacist>> GetAllAsync();
        Task<Pharmacist> GetByIdAsync(int id);
        Task AddAsync(Pharmacist pharmacist);
        Task UpdateAsync(Pharmacist pharmacist);
        Task DeleteAsync(int id);
    }
}
