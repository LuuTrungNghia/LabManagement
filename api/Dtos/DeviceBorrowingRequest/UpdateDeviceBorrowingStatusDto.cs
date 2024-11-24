public class UpdateDeviceBorrowingRequestDto
{
    public string Description { get; set; }
    public List<DeviceBorrowingDetailDto> DeviceBorrowingDetails { get; set; } = new List<DeviceBorrowingDetailDto>();
}
