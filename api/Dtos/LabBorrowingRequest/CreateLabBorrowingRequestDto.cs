public class CreateLabBorrowingRequestDto
{
    public string Username { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public List<DeviceBorrowingDetailDto> DeviceBorrowingDetails { get; set; }
}
