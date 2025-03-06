using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmacyAPI.Models;
using PharmacyAPI.Repository;
using System.Threading.Tasks;

namespace PharmacyAPI.Controllers
{
    [Route("api/v{version:apiVersion}/medicins")]
    [ApiController]
    [ApiVersion("1.0")]
    public class MedicinsController : ControllerBase
    {
        private readonly IMedicineRepository _repository;

        public MedicinsController(IMedicineRepository repository)//عملية حقن للانتر فيس الخاص بالريبوزتوري
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()//جلب 
        {
            var medicines = await _repository.GetAllAsync();
            return Ok(medicines);
        }

        [HttpGet("{id}")]//الجلب عن طريق ID
        public async Task<IActionResult> GetById(int id)
        {
            var medicine = await _repository.GetByIdAsync(id);
            if (medicine == null) return NotFound();
            return Ok(medicine);
        }

        [Authorize]
        [HttpPost]//انشاء
        public async Task<IActionResult> Create([FromBody] Medicine medicine)
        {
            await _repository.AddAsync(medicine);
            return CreatedAtAction(nameof(GetById), new { id = medicine.Id }, medicine);
        }

        [Authorize]//تعديل واستتطيع استخدام ال PATCH
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Medicine medicine)
        {
            if (id != medicine.Id) return BadRequest();
            await _repository.UpdateAsync(medicine);
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]//عملية الحذف
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _repository.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
