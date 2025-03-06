using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmacyAPI.Models;
using PharmacyAPI.Repository;
using System.Threading.Tasks;

namespace PharmacyAPI.Controllers
{
    [Route("api/v{version:apiVersion}/prescriptions")]
    [ApiController]
    [ApiVersion("1.0")]
    public class PrescriptionsController : ControllerBase
    {
        private readonly IPrescriptionRepository _repository;

        public PrescriptionsController(IPrescriptionRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var prescriptions = await _repository.GetAllAsync();
            return Ok(prescriptions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var prescription = await _repository.GetByIdAsync(id);
            if (prescription == null) return NotFound();
            return Ok(prescription);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Prescription prescription)
        {
            await _repository.AddAsync(prescription);
            return CreatedAtAction(nameof(GetById), new { id = prescription.Id }, prescription);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Prescription prescription)
        {
            if (id != prescription.Id) return BadRequest();
            await _repository.UpdateAsync(prescription);
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}
