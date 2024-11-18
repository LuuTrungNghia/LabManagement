using api.Models;

public class DeviceBorrowingRequest
{
    public int Id { get; set; }
    public string Username { get; set; } // Người đăng ký
    public string Description { get; set; } // Lý do mượn
    public DateTime FromDate { get; set; } // Ngày bắt đầu
    public DateTime ToDate { get; set; } // Ngày kết thúc
    public DeviceBorrowingStatus Status { get; set; } // Trạng thái đơn đăng ký

    // Mối quan hệ với User
    public string UserId { get; set; } // Khóa ngoại liên kết đến User
    public ApplicationUser User { get; set; } // Điều hướng đến User

    // Danh sách chi tiết thiết bị mượn
    public List<DeviceBorrowingDetail> DeviceBorrowingDetails { get; set; } = new List<DeviceBorrowingDetail>();
}

public class DeviceBorrowingDetail
    {
        public int Id { get; set; }

        public int DeviceBorrowingRequestId { get; set; } // FK đến DeviceBorrowingRequest
        public DeviceBorrowingRequest DeviceBorrowingRequest { get; set; } // Navigation property

        public int DeviceId { get; set; } // FK đến Device
        public Device Device { get; set; } // Navigation property

        public int DeviceItemId { get; set; } // Khóa ngoại đến DeviceItem
        public DeviceItem DeviceItem { get; set; } // Navigation property đến DeviceItem

        public string Description { get; set; } // Mô tả tình trạng thiết bị
    }

public enum DeviceBorrowingStatus
{
    Pending,
    Approved,
    Rejected,
    Completed
}
