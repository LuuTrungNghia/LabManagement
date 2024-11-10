using System.ComponentModel.DataAnnotations;
using api.Models;

namespace api.Dtos.Device
{
    public class UpdateDeviceRequestDto
    {
        [Required]
        public string DeviceName { get; set; } = string.Empty;

        [Required]
        public int CategoryId { get; set; }
        public DeviceItemStatus DeviceStatus { get; set; }
    }
}
