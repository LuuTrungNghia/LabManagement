using api.Models;

public class LabBorrowingRequestDto
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Description { get; set; }    
    public LabBorrowingStatus Status { get; set; }
    public List<GroupStudentDto> GroupStudents { get; set; }
    public List<DeviceBorrowingDetailDto> DeviceBorrowingDetails { get; set; }
}
