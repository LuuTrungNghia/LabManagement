using api.Dtos.Device;
using api.Models;

namespace api.Mappers
{
    public static class DeviceItemMapper
    {
        public static DeviceItem ToDeviceItem(this CreateDeviceItemDto dto) => new DeviceItem
        {
            DeviceItemName = dto.DeviceItemName,
            DeviceItemStatus = dto.DeviceItemStatus,
            Description = dto.Description
        };

        public static DeviceItemDto ToDeviceItemDto(this DeviceItem deviceItem) => new DeviceItemDto
        {
            DeviceItemId = deviceItem.DeviceItemId,
            DeviceItemName = deviceItem.DeviceItemName,
            DeviceItemStatus = deviceItem.DeviceItemStatus,
            Description = deviceItem.Description
        };
        
        public static DeviceItem ToDeviceItem(this UpdateDeviceItemDto dto, DeviceItem deviceItem)
        {
            deviceItem.DeviceItemName = dto.DeviceItemName;
            deviceItem.DeviceItemStatus = dto.DeviceItemStatus;
            deviceItem.Description = dto.Description;

            return deviceItem;
        }
    }
}
