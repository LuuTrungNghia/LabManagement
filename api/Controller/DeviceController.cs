using api.Dtos.Device;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace api.Controllers
{
    [Route("api/devices")]
    [ApiController]
    public class DevicesController : ControllerBase
    {
        private readonly IDeviceRepository _deviceRepo;
        private readonly ILogger<DevicesController> _logger;

        public DevicesController(IDeviceRepository deviceRepo, ILogger<DevicesController> logger)
        {
            _deviceRepo = deviceRepo;
            _logger = logger;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Fetching all devices.");
            var devices = await _deviceRepo.GetAllAsync();
            return Ok(devices.Select(d => d.ToDeviceDto()));
        }

        [HttpGet("get-by-id/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var device = await _deviceRepo.GetByIdAsync(id);
            if (device == null)
            {
                _logger.LogWarning("Device with ID {DeviceId} not found.", id);
                return NotFound(new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Device Not Found",
                    Detail = $"Device with ID {id} was not found.",
                    Instance = HttpContext.Request.Path
                });
            }

            return Ok(device.ToDeviceDto());
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateDeviceRequestDto deviceDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for creating a device.");
                return BadRequest(ModelState);
            }

            var device = deviceDto.ToDevice();
            await _deviceRepo.CreateAsync(device);

            _logger.LogInformation("Device created with ID {DeviceId}", device.Id);
            return CreatedAtAction(nameof(GetById), new { id = device.Id }, device);
        }

        [HttpPut("update/{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateDeviceRequestDto deviceDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for updating device with ID {DeviceId}.", id);
                return BadRequest(ModelState);
            }

            var updatedDevice = await _deviceRepo.UpdateAsync(id, deviceDto);
            if (updatedDevice == null)
            {
                _logger.LogWarning("Device with ID {DeviceId} not found for update.", id);
                return NotFound();
            }

            _logger.LogInformation("Device with ID {DeviceId} updated.", id);
            return Ok(updatedDevice.ToDeviceDto());
        }

        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deletedDevice = await _deviceRepo.DeleteAsync(id);
            if (deletedDevice == null)
            {
                _logger.LogWarning("Device with ID {DeviceId} not found for deletion.", id);
                return NotFound();
            }

            _logger.LogInformation("Device with ID {DeviceId} deleted.", id);
            return NoContent();
        }
    }
}
