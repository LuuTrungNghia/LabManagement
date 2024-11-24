using System.ComponentModel.DataAnnotations;

public class CreateDeviceBorrowingRequestDto
{
    [Required]
    public string Username { get; set; }

    [Required]
    [MaxLength(500)]
    public string Description { get; set; }
    [Required]
    public List<DeviceBorrowingDetailDto> DeviceBorrowingDetails { get; set; } = new List<DeviceBorrowingDetailDto>();
    public List<string> StudentUsernames { get; set; }
    public string LecturerUsername { get; set; }
}
