using api.Data;
using api.Dtos.Device;
using api.Helper;
using api.Interface;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Controller
{
    [Route("api/v{v}/Device")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly IDeviceRepository _deviceRepo;

        public DeviceController(IDeviceRepository deviceRepo)
        {
            _deviceRepo = deviceRepo;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var devices = await _deviceRepo.GetAllAsync();
            return Ok(devices.Select(s => s.ToDeviceDto()).ToList());
        }

        [HttpGet("get-device-by-id/{id:int}")]
        public async Task<IActionResult> GetDeviceById([FromRoute] int id)
        {
            var device = await _deviceRepo.GetDeviceByIdAsync(id);
            if (device == null)
                return NotFound();

            return Ok(device.ToDeviceDto());
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateDeviceRequestDto deviceDto)
        {
            var deviceModel = deviceDto.ToDeviceFromCreateDto();
            await _deviceRepo.CreateAsync(deviceModel);
            return CreatedAtAction(nameof(GetDeviceById), new { v = 1, id = deviceModel.Id }, deviceModel.ToDeviceDto());
        }

        [HttpPut("update-device/{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateDeviceRequestDto updateDto)
        {
            var deviceModel = await _deviceRepo.UpdateAsync(id, updateDto);
            if (deviceModel == null)
                return NotFound();

            return Ok(deviceModel.ToDeviceDto());
        }

        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var deviceModel = await _deviceRepo.DeleteAsync(id);
            if (deviceModel == null)
                return NotFound();

            return NoContent();
        }

        [HttpPost("import")]
        public async Task<IActionResult> ImportDevices([FromBody] List<CreateDeviceRequestDto> deviceDtos)
        {
            var devices = await _deviceRepo.ImportDevicesAsync(deviceDtos);
            return Ok(devices.Select(s => s.ToDeviceDto()).ToList());
        }
    }
}
