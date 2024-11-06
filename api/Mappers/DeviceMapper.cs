using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Device;
using api.Models;

namespace api.Mappers
{
    public static class DeviceMapper
    {
        public static DeviceDto ToDeviceDto(this Device deviceModel)
        {
            return new DeviceDto
            {
                Id = deviceModel.Id,
                DeviceName = deviceModel.DeviceName,
                Quantity = deviceModel.Quantity,
                DeviceStatus = deviceModel.DeviceStatus,
            };
        }

        public static Device ToDeviceFromCreateDto(this CreateDeviceRequestDto deviceDto)
        {
            return new Device
            {
                DeviceName = deviceDto.DeviceName,
                DeviceStatus = deviceDto.DeviceStatus,
                Quantity = deviceDto.Quantity,
            };
        }
    }
}