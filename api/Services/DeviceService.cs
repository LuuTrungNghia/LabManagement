using api.Dtos.Device;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.Extensions.Logging;

namespace api.Services
{
    public class DeviceService : IDeviceService
    {
        private readonly IDeviceRepository _deviceRepo;
        private readonly ICategoryRepository _categoryRepo;
        private readonly ILogger<DeviceService> _logger;

        public DeviceService(IDeviceRepository deviceRepo, ICategoryRepository categoryRepo, ILogger<DeviceService> logger)
        {
            _deviceRepo = deviceRepo;
            _categoryRepo = categoryRepo;
            _logger = logger;
        }

        public async Task<IEnumerable<DeviceDto>> GetAllDevicesAsync()
        {
            var devices = await _deviceRepo.GetAllAsync();
            var categories = await _categoryRepo.GetAllAsync();
            
            return devices.Select(d =>
            {
                var categoryName = categories.FirstOrDefault(c => c.CategoryId == d.CategoryId)?.CategoryName ?? "Unknown";
                return d.ToDeviceDto(categoryName);
            });
        }

        public async Task<DeviceDetailDto> GetDeviceByIdAsync(int id)
        {
            var device = await _deviceRepo.GetByIdAsync(id);
            if (device == null) return null;

            var category = await _categoryRepo.GetByIdAsync(device.CategoryId);
            var categoryName = category?.CategoryName ?? "Unknown";

            return device.ToDeviceDetailDto(categoryName);
        }

        public async Task<DeviceDto> CreateDeviceAsync(CreateDeviceRequestDto deviceDto)
        {
            var category = await _categoryRepo.GetByIdAsync(deviceDto.CategoryId);
            if (category == null) return null;

            var device = deviceDto.ToDevice();
            await _deviceRepo.CreateAsync(device);
            return device.ToDeviceDto(category.CategoryName);
        }

        public async Task<DeviceDto> UpdateDeviceAsync(int id, UpdateDeviceRequestDto deviceDto)
        {
            var updatedDevice = await _deviceRepo.UpdateAsync(id, deviceDto);
            if (updatedDevice == null) return null;

            var category = await _categoryRepo.GetByIdAsync(updatedDevice.CategoryId);
            var categoryName = category?.CategoryName ?? "Unknown";

            return updatedDevice.ToDeviceDto(categoryName);
        }

        public async Task<bool> DeleteDeviceAsync(int id)
        {
            var deletedDevice = await _deviceRepo.DeleteAsync(id);
            return deletedDevice != null;
        }

        public async Task<DeviceItemDto> AddDeviceItemAsync(int deviceId, CreateDeviceItemDto deviceItemDto)
        {
            var device = await _deviceRepo.GetByIdAsync(deviceId);
            if (device == null) return null;

            var deviceItem = DeviceItemMapper.ToDeviceItem(deviceItemDto);
            device.DeviceItems.Add(deviceItem);
            await _deviceRepo.UpdateAsync(deviceId, device.ToUpdateDeviceRequestDto());

            return deviceItem.ToDeviceItemDto();
        }

        public async Task<bool> DeleteDeviceItemAsync(int deviceId, int deviceItemId)
        {
            var device = await _deviceRepo.GetByIdAsync(deviceId);
            if (device == null) return false;

            var deviceItem = device.DeviceItems.FirstOrDefault(item => item.DeviceItemId == deviceItemId);
            if (deviceItem == null) return false;

            device.DeviceItems.Remove(deviceItem);
            await _deviceRepo.UpdateAsync(deviceId, device.ToUpdateDeviceRequestDto());
            return true;
        }

        public async Task<IEnumerable<DeviceDto>> ImportDevicesAsync(IEnumerable<CreateDeviceRequestDto> deviceDtos)
        {
            var devices = deviceDtos.Select(dto => dto.ToDevice()).ToList();
            await _deviceRepo.ImportDevices(devices);

            var categories = await _categoryRepo.GetAllAsync();
            return devices.Select(device =>
            {
                var categoryName = categories.FirstOrDefault(c => c.CategoryId == device.CategoryId)?.CategoryName ?? "Unknown";
                return device.ToDeviceDto(categoryName);
            });
        }
    }
}
