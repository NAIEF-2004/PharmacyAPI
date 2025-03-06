using System.Collections.Generic;
using System.Threading.Tasks;
using PharmacyAPI.Models;

namespace PharmacyAPI.Repository
{
    public interface IMedicineRepository
    {
        Task<IEnumerable<Medicine>> GetAllAsync();
        Task<Medicine> GetByIdAsync(int id);
        Task AddAsync(Medicine medicine);
        Task UpdateAsync(Medicine medicine);
        Task DeleteAsync(int id);
    }
}

