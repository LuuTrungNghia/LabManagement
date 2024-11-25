using api.Models;

namespace api.Dtos.Device
{
    public class DeviceItemDto
    {
        public int DeviceItemId { get; set; }

        public string DeviceItemName { get; set; } = string.Empty;

        public DeviceItemStatus DeviceItemStatus { get; set; } = DeviceItemStatus.Available;

        public string Description { get; set; } = string.Empty;
    }
}
