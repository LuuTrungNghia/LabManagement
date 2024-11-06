using api.Data;
using api.Dtos.Device;
using api.Helper;
using api.Interface;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controller
{
    [Route("api/v{v}/Device")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly IDeviceRepository _deviceRepo;
        public DeviceController(ApplicationDBContext context, IDeviceRepository deviceRepo)
        {
            _context = context;
            _deviceRepo = deviceRepo;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var devices = await _deviceRepo.GetAllAsync();
            var deviceDto = devices.Select(s => s.ToDeviceDto()).ToList();
            return Ok(deviceDto);
        }

        [HttpGet("get-device-by-id/{id:int}")]
        public async Task<IActionResult> GetDeviceById([FromRoute] int id)
        {
            var device = await _deviceRepo.GetDeviceByIdAsync(id);

            if (device == null)
            {
                return NotFound();
            }

            return Ok(device.ToDeviceDto());
        }

        [HttpGet("get-list")]
        public async Task<IActionResult> GetList([FromQuery] QueryObject query)
        {
            var devices = await _deviceRepo.GetListAsync(query);
            var deviceDto = devices.Select(s => s.ToDeviceDto()).ToList();
            return Ok(deviceDto);
        }


        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateDeviceRequestDto deviceDto)
        {
            var deviceModel = deviceDto.ToDeviceFromCreateDto();
            await _deviceRepo.CreateAsync(deviceModel);
            return CreatedAtAction(nameof(GetDeviceById), new{v = 1, id = deviceModel.Id}, deviceModel.ToDeviceDto());
        }

        [HttpPut("update-device/{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateDeviceRequestDto updateDto)
        {
            var deviceModel = await _deviceRepo.UpdateAsync(id, updateDto);

            if (deviceModel == null)
            {
                return NotFound();
            }

            return Ok(deviceModel.ToDeviceDto());
        }

        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var deviceModel = _deviceRepo.DeleteAsync(id);

            if (deviceModel == null) 
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}