using api.Dtos.Device;
using api.Models;

namespace api.Mappers
{
    public static class DeviceMapper
    {
        public static DeviceDto ToDeviceDto(this Device device) => new DeviceDto
        {
            Id = device.DeviceId,
            DeviceName = device.DeviceName,
            Total = device.Total,
            CategoryId = device.CategoryId,
            DeviceItems = device.DeviceItems.Select(item => item.ToDeviceItemDto()).ToList()
        };

        public static Device ToDevice(this CreateDeviceRequestDto dto) => new Device
        {
            DeviceName = dto.DeviceName,
            CategoryId = dto.CategoryId
        };

        public static Device ToDevice(this UpdateDeviceRequestDto dto, Device device)
        {
            device.DeviceName = dto.DeviceName;
            device.CategoryId = dto.CategoryId;
            return device;
        }

        public static UpdateDeviceRequestDto ToUpdateDeviceRequestDto(this Device device) => new UpdateDeviceRequestDto
        {
            DeviceName = device.DeviceName,
            CategoryId = device.CategoryId
        };

        public static DeviceItem ToDeviceItem(this CreateDeviceItemDto dto) => new DeviceItem
        {
            DeviceItemName = dto.DeviceItemName,
            DeviceItemStatus = dto.DeviceItemStatus,
            Description = dto.Description
        };
    }
}
