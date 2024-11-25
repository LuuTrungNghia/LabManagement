public class LabBorrowingRequest
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public LabBorrowingStatus Status { get; set; }
    public int LabId { get; set; } = 1;
    public Lab BorrowedLab { get; set; }
    public List<DeviceBorrowingDetail> DeviceBorrowingDetails { get; set; } = new();
}

public enum LabBorrowingStatus
{
    Pending,
    Approved,
    Rejected
}
