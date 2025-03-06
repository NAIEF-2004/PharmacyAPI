using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmacyAPI.Models;
using PharmacyAPI.Repository;

namespace PharmacyAPI.Controllers
{
    [Route("api/v{version:apiVersion}/pharmacists")]
    [ApiController]
    [ApiVersion("1.0")]

    public class PharmacistsController : ControllerBase
    {
        private readonly IPharmacistRepository _repository;

        public PharmacistsController(IPharmacistRepository repository)
        {
            _repository = repository;
        }

       //عملية عرض كل مال\ي من صيدليات
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var pharmacists = await _repository.GetAllAsync();
            return Ok(pharmacists);
        }

       //الجلب عن طريق الايدي
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var pharmacist = await _repository.GetByIdAsync(id);
            if (pharmacist == null) return NotFound();
            return Ok(pharmacist);
        }

     //انشاء او اضافة مع تطلب المصادقة 
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Pharmacist pharmacist)
        {
            await _repository.AddAsync(pharmacist);
            return CreatedAtAction(nameof(GetById), new { id = pharmacist.Id }, pharmacist);
        }

       //تعديل ايضا يتطلب المصادقة
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Pharmacist pharmacist)
        {
            if (id != pharmacist.Id) return BadRequest();
            await _repository.UpdateAsync(pharmacist);
            return NoContent();
        }

        //حذف مع طلب المصادقة 
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}