public class LabBorrowingRequestDto
{
    public int Id { get; set; }
    public string Username { get; set; }    
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Description { get; set; }
    public LabBorrowingStatus Status { get; set; }
    public List<DeviceBorrowingDetailDto> DeviceBorrowingDetails { get; set; }
}
