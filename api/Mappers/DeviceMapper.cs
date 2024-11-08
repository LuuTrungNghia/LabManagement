using api.Dtos.Device;
using api.Models;

namespace api.Mappers
{
    public static class DeviceMappers
    {
        public static DeviceDto ToDeviceDto(this Device device) => new DeviceDto
        {
            Id = device.Id,
            DeviceName = device.DeviceName,
            Quantity = device.Quantity,
            DeviceStatus = device.DeviceStatus.ToString()
        };

        public static Device ToDevice(this CreateDeviceRequestDto dto) => new Device
        {
            DeviceName = dto.DeviceName,
            Quantity = dto.Quantity,
            DeviceStatus = DeviceStatus.Good
        };

        public static Device ToDevice(this UpdateDeviceRequestDto dto, Device device)
        {
            device.DeviceName = dto.DeviceName;
            device.Quantity = dto.Quantity;
            device.DeviceStatus = Enum.Parse<DeviceStatus>(dto.DeviceStatus);
            return device;
        }
    }
}
