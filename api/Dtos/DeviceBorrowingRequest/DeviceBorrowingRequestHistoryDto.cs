public class DeviceBorrowingRequestHistoryDto
{
    public int Id { get; set; } // ID của lịch sử
    public string Username { get; set; } // Người mượn
    public string Description { get; set; } // Lý do mượn
    public DateTime FromDate { get; set; } // Ngày bắt đầu
    public DateTime ToDate { get; set; } // Ngày kết thúc
    public DeviceBorrowingStatus Status { get; set; } // Trạng thái yêu cầu

    // Danh sách chi tiết mượn thiết bị trong lịch sử
    public List<DeviceBorrowingDetailDto> DeviceBorrowingDetails { get; set; } = new List<DeviceBorrowingDetailDto>();
}
