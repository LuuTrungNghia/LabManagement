// using api.Dtos;
// using api.Services;
// using Microsoft.AspNetCore.Mvc;
// using System.Threading.Tasks;

// namespace api.Controllers
// {
//     [Route("api/[controller]")]
//     [ApiController]
//     public class LabsController : ControllerBase
//     {
//         private readonly ILabService _labService;

//         public LabsController(ILabService labService)
//         {
//             _labService = labService;
//         }

//         [HttpGet]
//         public async Task<IActionResult> GetAllLabs()
//         {
//             var labs = await _labService.GetAllLabsAsync();
//             return Ok(labs);
//         }

//         [HttpGet("{id}")]
//         public async Task<IActionResult> GetLabById(int id)
//         {
//             var lab = await _labService.GetLabByIdAsync(id);
//             if (lab == null) return NotFound();
//             return Ok(lab);
//         }

//         [HttpPost]
//         public async Task<IActionResult> CreateLab(CreateLabDto createLabDto)
//         {
//             await _labService.CreateLabAsync(createLabDto);
//             return CreatedAtAction(nameof(GetLabById), new { id = createLabDto.LabName }, createLabDto);
//         }

//         [HttpPut("{id}")]
//         public async Task<IActionResult> UpdateLab(int id, UpdateLabDto updateLabDto)
//         {
//             await _labService.UpdateLabAsync(id, updateLabDto);
//             return NoContent();
//         }

//         [HttpDelete("{id}")]
//         public async Task<IActionResult> DeleteLab(int id)
//         {
//             await _labService.DeleteLabAsync(id);
//             return NoContent();
//         }
//     }
// }
