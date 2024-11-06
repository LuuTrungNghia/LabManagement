using api.Data;
using api.Dtos.Lab;
using api.Helper;
using api.Interface;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/v{v}/Lab")]
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

        [HttpGet("get/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var lab = await _labRepo.GetByIdAsync(id);
            if (lab == null) return NotFound();
            return Ok(lab);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateLabRequestDto labDto)
        {
            var lab = await _labRepo.CreateAsync(labDto);
            return CreatedAtAction(nameof(GetById), new { v = 1, id = lab.Id }, lab);
        }

        [HttpPut("update/{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateLabRequestDto labDto)
        {
            var updatedLab = await _labRepo.UpdateAsync(id, labDto);
            if (updatedLab == null) return NotFound();
            return Ok(updatedLab);
        }

        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _labRepo.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
