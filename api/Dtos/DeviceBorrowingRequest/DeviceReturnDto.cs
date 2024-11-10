using System.ComponentModel.DataAnnotations;

namespace api.Dtos.DeviceBorrowing
{
    public class DeviceReturnDto
    {
        [Required]
        public int DeviceId { get; set; }

        [Required]
        public int DeviceItemId { get; set; }
        public string Condition { get; set; } = string.Empty;
    }
}
