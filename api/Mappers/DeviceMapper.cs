using api.Dtos.Device;
using api.Models;
using System.Collections.Generic;
using System.Linq;

namespace api.Mappers
{
    public static class DeviceMappers
    {
        // Map Device model to DeviceDto
        public static DeviceDto ToDeviceDto(this Device device)
        {
            return new DeviceDto
            {
                Id = device.Id,
                DeviceName = device.DeviceName,
                Quantity = device.Quantity,
                DeviceStatus = device.DeviceStatus
            };
        }

        // Map CreateDeviceRequestDto to Device model
        public static Device ToDeviceFromCreateDto(this CreateDeviceRequestDto deviceDto)
        {
            return new Device
            {
                DeviceName = deviceDto.DeviceName,
                Quantity = deviceDto.Quantity,
                DeviceStatus = "Good" // Default to "Good" if not specified
            };
        }

        // Map UpdateDeviceRequestDto to Device model (for updating existing devices)
        public static void UpdateDeviceFromDto(this Device existingDevice, UpdateDeviceRequestDto deviceDto)
        {
            existingDevice.DeviceName = deviceDto.DeviceName;
            existingDevice.Quantity = deviceDto.Quantity;
            existingDevice.DeviceStatus = string.IsNullOrEmpty(deviceDto.DeviceStatus) ? existingDevice.DeviceStatus : deviceDto.DeviceStatus;
        }

        // Map a list of Device models to a list of DeviceDto
        public static List<DeviceDto> ToDeviceDtoList(this IEnumerable<Device> devices)
        {
            return devices.Select(device => device.ToDeviceDto()).ToList();
        }
    }
}
