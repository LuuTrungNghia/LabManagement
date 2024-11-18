using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public class DeviceItem
    {
        public int DeviceItemId { get; set; }

        [Required]
        public string DeviceItemName { get; set; }

        [Required]
        public DeviceItemStatus DeviceItemStatus { get; set; } = DeviceItemStatus.Available;

        public string Description { get; set; } = string.Empty;

        [Required]
        public int DeviceId { get; set; }

        [ForeignKey("DeviceId")]
        public Device Device { get; set; }
        public int? BorrowedByUserId { get; set; }
    }
    public enum DeviceItemStatus
    {        
        Broken,
        Available,
        Borrowed
    }
}
