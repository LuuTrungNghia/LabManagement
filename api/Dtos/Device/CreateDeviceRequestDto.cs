using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Device
{
    public class CreateDeviceRequestDto
    {
        [Required]
        public string DeviceName { get; set; } = string.Empty;

        [Required]
        public int CategoryId { get; set; }
    }
}
