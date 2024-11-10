using System.ComponentModel.DataAnnotations;

namespace api.Dtos.DeviceBorrowing
{
    public class UpdateRequestStatusDto
    {
        [Required]
        public int RequestId { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; }
    }
}
