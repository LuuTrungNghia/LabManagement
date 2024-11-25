using System.ComponentModel.DataAnnotations;

public class CreateDeviceBorrowingRequestDto
{
    [Required]
    [MaxLength(500)]
    public string Description { get; set; }
    public List<GroupStudentDto> GroupStudents { get; set; } = new List<GroupStudentDto>();
    [Required]
    public List<DeviceBorrowingDetailDto> DeviceBorrowingDetails { get; set; } = new List<DeviceBorrowingDetailDto>();
}
