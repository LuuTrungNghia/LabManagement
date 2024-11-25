public class BaseBorrowingRequestDto
{
    public string Username { get; set; }
    public string Description { get; set; }
    public List<GroupStudentDto> GroupStudents { get; set; }
}