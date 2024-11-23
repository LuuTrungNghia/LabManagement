public class LabBorrowingRequestDto
{
    public int LabBorrowingRequestId { get; set; }
    public int LabId { get; set; }
    public string LabName { get; set; } = string.Empty;
    public string UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public bool IsApproved { get; set; }
    public List<int> DeviceIds { get; set; } = new List<int>();
}
