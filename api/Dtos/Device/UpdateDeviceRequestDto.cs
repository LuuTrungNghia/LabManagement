using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Device
{
    public class UpdateDeviceRequestDto
    {
        [Required]
        public string DeviceName { get; set; } = string.Empty;
        [Required]
        public int Quantity { get; set; }
        public string DeviceStatus { get; set; } = string.Empty;
    }
}
