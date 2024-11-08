using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    [Table("Devices")]
    public class Device
    {
        public int Id { get; set; }
        [Required]
        public string DeviceName { get; set; } = string.Empty;
        [Required]
        public int Quantity { get; set; }
        [Required]
        public string DeviceStatus { get; set; } = "Good"; // Tình trạng thiết bị (Good, Broken, Borrowed, Available)
    }
}
