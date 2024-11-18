public class DeviceBorrowingRequestDto
{
    public int Id { get; set; }
    public string Username { get; set; } // Người đăng ký
    public string Description { get; set; } // Lý do mượn
    public DateTime FromDate { get; set; } // Ngày bắt đầu
    public DateTime ToDate { get; set; } // Ngày kết thúc
    public DeviceBorrowingStatus Status { get; set; } // Trạng thái đơn đăng ký

    // Danh sách chi tiết thiết bị mượn
    public List<DeviceBorrowingDetailDto> DeviceBorrowingDetails { get; set; } = new List<DeviceBorrowingDetailDto>();
}
