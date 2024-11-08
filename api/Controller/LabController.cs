using api.Dtos.Lab;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/lab")]
    [ApiController]
    public class LabController : ControllerBase
    {
        private readonly ILabRepository _labRepo;

        public LabController(ILabRepository labRepo)
        {
            _labRepo = labRepo;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var labs = await _labRepo.GetAllAsync();
            return Ok(labs);
        }

        [HttpGet("get-by-id/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var lab = await _labRepo.GetByIdAsync(id);
            if (lab == null) return NotFound();

            return Ok(lab);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateLabRequestDto labDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var lab = new Lab
            {
                LabName = labDto.LabName,
                Description = labDto.Description,
                Location = labDto.Location,
                IsAvailable = labDto.IsAvailable
            };

            await _labRepo.CreateAsync(lab);
            return CreatedAtAction(nameof(GetById), new { id = lab.Id }, lab);
        }

        [HttpPut("update/{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateLabRequestDto labDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existingLab = await _labRepo.GetByIdAsync(id);
            if (existingLab == null) return NotFound();

            existingLab.LabName = labDto.LabName;
            existingLab.Description = labDto.Description;
            existingLab.Location = labDto.Location;
            existingLab.IsAvailable = labDto.IsAvailable;

            await _labRepo.UpdateAsync(existingLab);
            return NoContent();
        }

        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existingLab = await _labRepo.GetByIdAsync(id);
            if (existingLab == null) return NotFound();

            await _labRepo.DeleteAsync(id);
            return NoContent();
        }
    }
}
