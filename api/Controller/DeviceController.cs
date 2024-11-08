using api.Dtos.Device;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/devices")]
    [ApiController]
    public class DevicesController : ControllerBase
    {
        private readonly IDeviceRepository _deviceRepo;

        public DevicesController(IDeviceRepository deviceRepo)
        {
            _deviceRepo = deviceRepo;
        }

        [HttpGet("get-all")]
        // [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAll()
        {
            var devices = await _deviceRepo.GetAllAsync();
            var deviceDtos = devices.Select(d => new DeviceDto
            {
                Id = d.Id,
                DeviceName = d.DeviceName,
                Quantity = d.Quantity,
                DeviceStatus = d.DeviceStatus
            }).ToList();

            return Ok(deviceDtos);
        }

        [HttpGet("get-by-id/{id:int}")]
        // [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetById(int id)
        {
            var device = await _deviceRepo.GetByIdAsync(id);
            if (device == null) return NotFound();

            var deviceDto = new DeviceDto
            {
                Id = device.Id,
                DeviceName = device.DeviceName,
                Quantity = device.Quantity,
                DeviceStatus = device.DeviceStatus
            };

            return Ok(deviceDto);
        }

        [HttpPost("create")]
        // [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create([FromBody] CreateDeviceRequestDto deviceDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var device = new Device
            {
                DeviceName = deviceDto.DeviceName,
                Quantity = deviceDto.Quantity
            };

            await _deviceRepo.CreateAsync(device);
            return CreatedAtAction(nameof(GetById), new { id = device.Id }, device);
        }

        [HttpPut("update/{id:int}")]
        // [Authorize(Roles = "admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateDeviceRequestDto deviceDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updatedDevice = await _deviceRepo.UpdateAsync(id, deviceDto);
            if (updatedDevice == null) return NotFound();

            return Ok(updatedDevice);
        }

        [HttpDelete("delete/{id:int}")]
        // [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var deletedDevice = await _deviceRepo.DeleteAsync(id);
            if (deletedDevice == null) return NotFound();

            return NoContent();
        }

        [HttpPost("import")]
        // [Authorize(Roles = "admin")]
        public async Task<IActionResult> Import([FromBody] List<CreateDeviceRequestDto> deviceDtos)
        {
            var devices = deviceDtos.Select(dto => new Device
            {
                DeviceName = dto.DeviceName,
                Quantity = dto.Quantity
            }).ToList();

            await _deviceRepo.ImportDevices(devices);
            return Ok();
        }
    }
}
