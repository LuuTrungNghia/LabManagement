using System.ComponentModel.DataAnnotations;
using api.Models;

namespace api.Dtos.Device
{
    public class UpdateDeviceItemDto
    {
        [Required]
        public string DeviceItemName { get; set; } = string.Empty;

        [Required]
        public DeviceItemStatus DeviceItemStatus { get; set; } = DeviceItemStatus.Available;

        public string Description { get; set; } = string.Empty;
    }
}
