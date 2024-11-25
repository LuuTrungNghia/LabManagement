public class CreateLabBorrowingRequestDto
{
    public string Username { get; set; }
    public string Description { get; set; }
    public List<GroupStudentDto> GroupStudents { get; set; }
    public List<DeviceBorrowingDetailDto> DeviceBorrowingDetails { get; set; }
}
