using api.Dtos.Device;

namespace api.Interfaces
{
    public interface IDeviceService
    {
        Task<IEnumerable<DeviceDto>> GetAllDevicesAsync();
        Task<DeviceDetailDto> GetDeviceByIdAsync(int id);
        Task<DeviceDto> CreateDeviceAsync(CreateDeviceRequestDto deviceDto);
        Task<DeviceDto> UpdateDeviceAsync(int id, UpdateDeviceRequestDto deviceDto);
        Task<bool> DeleteDeviceAsync(int id);
        Task<DeviceItemDto> AddDeviceItemAsync(int deviceId, CreateDeviceItemDto deviceItemDto);
        Task<bool> DeleteDeviceItemAsync(int deviceId, int deviceItemId);
        Task<IEnumerable<DeviceDto>> ImportDevicesAsync(IEnumerable<CreateDeviceRequestDto> deviceDtos);
    }
}
