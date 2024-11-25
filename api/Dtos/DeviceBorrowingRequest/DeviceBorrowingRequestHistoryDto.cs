public class DeviceBorrowingRequestHistoryDto
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Description { get; set; } 
    public DeviceBorrowingStatus Status { get; set; } 
    public List<GroupStudentDto> GroupStudents { get; set; } = new List<GroupStudentDto>();
    public List<DeviceBorrowingDetailDto> DeviceBorrowingDetails { get; set; } = new List<DeviceBorrowingDetailDto>();
}
