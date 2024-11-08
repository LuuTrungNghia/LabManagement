using System.ComponentModel.DataAnnotations;

namespace api.Dtos.DeviceBorrowing
{
    public class CreateDeviceBorrowingRequestDto
    {
        [Required]
        public int DeviceId { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public string BorrowerType { get; set; }
    }
}