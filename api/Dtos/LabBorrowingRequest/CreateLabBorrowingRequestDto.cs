public class CreateLabBorrowingRequestDto
{
    public string Username { get; set; }
    public string Description { get; set; }
    public List<GroupStudentDto> GroupStudents { get; set; }
    public List<CreateDeviceBorrowingRequestDto> DeviceBorrowingRequests { get; set; }
}