public class LabBorrowingRequest
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Description { get; set; }
    public LabBorrowingStatus Status { get; set; }
    public int LabId { get; set; } = 1;
    public Lab BorrowedLab { get; set; }
    public List<GroupStudent> GroupStudents { get; set; }
    public List<DeviceBorrowingRequest> DeviceBorrowingRequests { get; set; }
}

public enum LabBorrowingStatus
{
    Pending,
    Approved,
    Rejected
}
