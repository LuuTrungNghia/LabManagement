using api.Models;

public class LabBorrowingRequestDto : BaseBorrowingRequestDto
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Description { get; set; }    
    public LabBorrowingStatus Status { get; set; }
    public List<GroupStudentDto> GroupStudents { get; set; }
    public List<DeviceBorrowingRequestDto> DeviceBorrowingRequests { get; set; }
}
