using System.Collections.Generic;
using System.Threading.Tasks;
using api.Dtos;
using api.Services;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabBorrowingRequestsController : ControllerBase
    {
        private readonly ILabBorrowingRequestService _service;

        public LabBorrowingRequestsController(ILabBorrowingRequestService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LabBorrowingRequestDto>>> GetAllLabBorrowingRequests()
        {
            var requests = await _service.GetAllLabBorrowingRequestsAsync();
            return Ok(requests);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LabBorrowingRequestDto>> GetLabBorrowingRequestById(int id)
        {
            var request = await _service.GetLabBorrowingRequestByIdAsync(id);
            if (request == null) return NotFound();
            return Ok(request);
        }

        [HttpPost]
        public async Task<ActionResult<LabBorrowingRequestDto>> CreateLabBorrowingRequest(CreateLabBorrowingRequestDto dto)
        {
            var createdRequest = await _service.CreateLabBorrowingRequestAsync(dto);
            return CreatedAtAction(nameof(GetLabBorrowingRequestById), new { id = createdRequest.Id }, createdRequest);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<LabBorrowingRequestDto>> UpdateLabBorrowingRequest(int id, UpdateLabBorrowingRequestDto dto)
        {
            var updatedRequest = await _service.UpdateLabBorrowingRequestAsync(id, dto);
            if (updatedRequest == null) return NotFound();
            return Ok(updatedRequest);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteLabBorrowingRequest(int id)
        {
            var success = await _service.DeleteLabBorrowingRequestAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
        
        [HttpPost("approve/{id}")]
        public async Task<ActionResult<LabBorrowingRequestDto>> ApproveLabBorrowingRequest(int id)
        {
            var updatedRequest = await _service.ApproveLabBorrowingRequestAsync(id);
            if (updatedRequest == null) return NotFound();
            return Ok(updatedRequest);
        }

        [HttpPost("reject/{id}")]
        public async Task<ActionResult<LabBorrowingRequestDto>> RejectLabBorrowingRequest(int id)
        {
            var updatedRequest = await _service.RejectLabBorrowingRequestAsync(id);
            if (updatedRequest == null) return NotFound();
            return Ok(updatedRequest);
        }
    }
}
