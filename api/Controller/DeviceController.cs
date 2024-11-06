using api.Data;
using api.Dtos.Device;
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

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateDeviceRequestDto deviceDto)
        {
            var deviceModel = deviceDto.ToDeviceFromCreateDto();
            await _deviceRepo.CreateAsync(deviceModel);
            return CreatedAtAction(nameof(GetDeviceById), new{v = 1, id = deviceModel.Id}, deviceModel.ToDeviceDto());
        }
    }
}