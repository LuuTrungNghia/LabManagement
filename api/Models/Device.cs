using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public class Device
    {
        public int DeviceId { get; set; }

        [Required]
        public string DeviceName { get; set; } = string.Empty;

        public int Total => DeviceItems?.Count ?? 0;

        public int AvailableQuantity => DeviceItems?.Count(item => item.DeviceItemStatus == DeviceItemStatus.Available) ?? 0;

        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        public List<DeviceItem> DeviceItems { get; set; } = new();

        public DeviceItemStatus DeviceStatus { get; set; } = DeviceItemStatus.Available;

        public List<DeviceBorrowingDetail> DeviceBorrowingDetails { get; set; } = new List<DeviceBorrowingDetail>();
    }
}
