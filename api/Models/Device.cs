using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    public class Device
    {
        public int Id { get; set; }

        [Required]
        public string DeviceName { get; set; } = string.Empty;

        [Required]
        public int Quantity { get; set; }

        [Required]
        public DeviceStatus DeviceStatus { get; set; }
    }
}
