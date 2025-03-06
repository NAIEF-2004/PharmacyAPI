using Microsoft.EntityFrameworkCore;
using PharmacyAPI.Models;

namespace PharmacyAPI.Data
{
    public class PharmacyDbContext:DbContext//القاعدة الخاصة بالصيدليات والمعلومات
    {

        public DbSet<Pharmacist> Pharmacists { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }

        public PharmacyDbContext(DbContextOptions<PharmacyDbContext> options) : base(options) {
        
        }

    }
}

