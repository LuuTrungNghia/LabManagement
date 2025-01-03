using api.Dtos.Device;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace api.Controllers
{
    [Route("api/devices")]
    [ApiController]
    public class DevicesController : ControllerBase
    {
        private readonly IDeviceRepository _deviceRepo;
        private readonly ICategoryRepository _categoryRepo;
        private readonly ILogger<DevicesController> _logger;

        public DevicesController(IDeviceRepository deviceRepo, ICategoryRepository categoryRepo, ILogger<DevicesController> logger)
        {
            _deviceRepo = deviceRepo;
            _categoryRepo = categoryRepo;
            _logger = logger;
        }

        [HttpGet("get-all")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Fetching all devices.");
            
            var devices = await _deviceRepo.GetAllAsync();
            var categories = await _categoryRepo.GetAllAsync();
            
            var result = devices.Select(d => 
            {
                var categoryName = categories.FirstOrDefault(c => c.CategoryId == d.CategoryId)?.CategoryName ?? "Unknown";
                return d.ToDeviceDto(categoryName);
            });

            return Ok(result);
        }

        [HttpGet("get-by-id/{id:int}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetById(int id)
        {
            // Lấy thiết bị theo ID
            var device = await _deviceRepo.GetByIdAsync(id);
            if (device == null)
            {
                // Nếu không tìm thấy thiết bị, trả về lỗi 404
                _logger.LogWarning("Device with ID {DeviceId} not found.", id);
                return NotFound();
            }

            // Lấy thông tin danh mục của thiết bị
            var category = await _categoryRepo.GetByIdAsync(device.CategoryId);
            var categoryName = category?.CategoryName ?? "Unknown";

            // Trả về thông tin chi tiết của thiết bị, bao gồm tên danh mục
            return Ok(device.ToDeviceDetailDto(categoryName));
        }

        [HttpPost("create")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create([FromBody] CreateDeviceRequestDto deviceDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for creating a device.");
                return BadRequest(ModelState);
            }
            
            var category = await _categoryRepo.GetByIdAsync(deviceDto.CategoryId);
            if (category == null)
            {
                _logger.LogWarning("Category with ID {CategoryId} not found.", deviceDto.CategoryId);
                return BadRequest($"Category with ID {deviceDto.CategoryId} not found.");
            }

            var device = deviceDto.ToDevice();
            await _deviceRepo.CreateAsync(device);

            _logger.LogInformation("Device created with ID {DeviceId}", device.DeviceId);

            // Include CategoryName in the response
            return CreatedAtAction(nameof(GetById), new { id = device.DeviceId }, device.ToDeviceDto(category.CategoryName));
        }

        [HttpPost("{deviceId:int}/add-device-item")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AddDeviceItem(int deviceId, [FromBody] CreateDeviceItemDto deviceItemDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for adding a device item.");
                return BadRequest(ModelState);
            }

            var device = await _deviceRepo.GetByIdAsync(deviceId);
            if (device == null)
            {
                _logger.LogWarning("Device with ID {DeviceId} not found for adding item.", deviceId);
                return NotFound();
            }

            var deviceItem = DeviceItemMapper.ToDeviceItem(deviceItemDto);
            device.DeviceItems.Add(deviceItem);
            await _deviceRepo.UpdateAsync(deviceId, device.ToUpdateDeviceRequestDto());

            _logger.LogInformation("Device item added to device with ID {DeviceId}", deviceId);
            return Ok(deviceItem.ToDeviceItemDto());
        }
        
        [HttpDelete("delete-device-item/{deviceId:int}/{deviceItemId:int}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteDeviceItem(int deviceId, int deviceItemId)
        {
            var device = await _deviceRepo.GetByIdAsync(deviceId);
            if (device == null)
            {
                _logger.LogWarning("Device with ID {DeviceId} not found for deleting item.", deviceId);
                return NotFound();
            }

            var deviceItem = device.DeviceItems.FirstOrDefault(item => item.DeviceItemId == deviceItemId);
            if (deviceItem == null)
            {
                _logger.LogWarning("Device item with ID {DeviceItemId} not found in device with ID {DeviceId}.", deviceItemId, deviceId);
                return NotFound();
            }

            // Remove the device item from the device
            device.DeviceItems.Remove(deviceItem);
            await _deviceRepo.DeleteAsync(deviceItem.DeviceItemId);
            await _deviceRepo.UpdateAsync(deviceId, device.ToUpdateDeviceRequestDto());

            // No need to explicitly delete the DeviceItem if cascading delete is set up
            _logger.LogInformation("Device item with ID {DeviceItemId} deleted from device with ID {DeviceId}", deviceItemId, deviceId);
            return NoContent();
        }

        [HttpGet("categories")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAllCategories()
        {
            _logger.LogInformation("Fetching all categories.");

            var categories = await _categoryRepo.GetAllAsync();
            var categoryDtos = categories.Select(category => category.ToCategoryDto());

            _logger.LogInformation("{Count} categories fetched successfully.", categoryDtos.Count());
            return Ok(categoryDtos);
        }

        [HttpPost("add-category")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AddCategory([FromBody] CreateCategoryDto categoryDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for adding a category.");
                return BadRequest(ModelState);
            }

            var category = categoryDto.ToCategory();
            await _categoryRepo.CreateAsync(category);

            _logger.LogInformation("Category created with ID {CategoryId}", category.CategoryId);

            // Correct the route reference to the CategoriesController's GetCategoryById action
            return CreatedAtAction(nameof(GetCategoryById), new { categoryId = category.CategoryId }, category.ToCategoryDto());
        }

        [HttpGet("category/{categoryId:int}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetCategoryById(int categoryId)
        {
            var category = await _categoryRepo.GetByIdAsync(categoryId);
            if (category == null)
            {
                _logger.LogWarning("Category with ID {CategoryId} not found.", categoryId);
                return NotFound();
            }

            return Ok(category.ToCategoryDto());
        }

        [HttpPut("update/{id:int}")]
        [Authorize(Roles = "admin")]
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

            // Lấy tên danh mục mới từ CategoryRepository
            var category = await _categoryRepo.GetByIdAsync(updatedDevice.CategoryId);
            var categoryName = category?.CategoryName ?? "Unknown";

            _logger.LogInformation("Device with ID {DeviceId} updated.", id);
            return Ok(updatedDevice.ToDeviceDto(categoryName)); // Truyền categoryName vào
        }

        [HttpPut("{deviceId:int}/update-device-item/{deviceItemId:int}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateDeviceItem(int deviceId, int deviceItemId, [FromBody] UpdateDeviceItemDto deviceItemDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for updating device item with ID {DeviceItemId}.", deviceItemId);
                return BadRequest(ModelState);
            }

            var device = await _deviceRepo.GetByIdAsync(deviceId);
            if (device == null)
            {
                _logger.LogWarning("Device with ID {DeviceId} not found for updating item.", deviceId);
                return NotFound();
            }

            var existingDeviceItem = device.DeviceItems.FirstOrDefault(item => item.DeviceItemId == deviceItemId);
            if (existingDeviceItem == null)
            {
                _logger.LogWarning("Device item with ID {DeviceItemId} not found in device with ID {DeviceId}.", deviceItemId, deviceId);
                return NotFound();
            }

            // Update the device item properties
            existingDeviceItem.DeviceItemName = deviceItemDto.DeviceItemName;
            existingDeviceItem.DeviceItemStatus = deviceItemDto.DeviceItemStatus;
            existingDeviceItem.Description = deviceItemDto.Description;

            // Save the updated device to the repository
            await _deviceRepo.UpdateAsync(deviceId, device.ToUpdateDeviceRequestDto());

            _logger.LogInformation("Device item with ID {DeviceItemId} updated in device with ID {DeviceId}.", deviceItemId, deviceId);
            return Ok(existingDeviceItem.ToDeviceItemDto());
        }

        [HttpDelete("delete/{id:int}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var device = await _deviceRepo.GetByIdAsync(id);
            if (device == null)
            {
                _logger.LogWarning("Device with ID {DeviceId} not found for deletion.", id);
                return NotFound();
            }

            // Manually delete related DeviceItems if necessary
            foreach (var deviceItem in device.DeviceItems.ToList())
            {
                await _deviceRepo.DeleteAsync(deviceItem.DeviceItemId);
            }

            // Delete the device
            await _deviceRepo.DeleteAsync(id);

            _logger.LogInformation("Device with ID {DeviceId} deleted.", id);
            return NoContent();
        }

        [HttpPost("import")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ImportDevices([FromBody] IEnumerable<CreateDeviceRequestDto> deviceDtos)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for importing devices.");
                return BadRequest(ModelState);
            }

            var devices = deviceDtos.Select(dto => dto.ToDevice()).ToList();
            await _deviceRepo.ImportDevices(devices);

            // Lấy tất cả danh mục để sử dụng trong việc chuyển đổi
            var categories = await _categoryRepo.GetAllAsync();
            
            var result = devices.Select(device =>
            {
                var categoryName = categories.FirstOrDefault(c => c.CategoryId == device.CategoryId)?.CategoryName ?? "Unknown";
                return device.ToDeviceDto(categoryName); // Truyền categoryName vào
            });

            _logger.LogInformation("{Count} devices imported successfully.", devices.Count);
            return Ok(result);
        }
    }
}