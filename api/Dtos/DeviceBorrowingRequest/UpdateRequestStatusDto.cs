using System.ComponentModel.DataAnnotations;

namespace api.Dtos.DeviceBorrowingRequest
{
    public class UpdateRequestStatusDto
    {
        [Required]
        public int RequestId { get; set; }

        [Required]
        [StringLength(50)]
        [RegularExpression("^(Pending|Approved|Returned)$", ErrorMessage = "Invalid status")]
        public string Status { get; set; }
    }
}
