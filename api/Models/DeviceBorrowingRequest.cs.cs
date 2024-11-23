using api.Models;

public class DeviceBorrowingRequest
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Description { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public DeviceBorrowingStatus Status { get; set; }

    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
    public List<DeviceBorrowingDetail> DeviceBorrowingDetails { get; set; } = new List<DeviceBorrowingDetail>();
}

public class DeviceBorrowingDetail
    {
        public int Id { get; set; }

        public int DeviceBorrowingRequestId { get; set; }
        public DeviceBorrowingRequest DeviceBorrowingRequest { get; set; }

        public int DeviceId { get; set; }
        public Device Device { get; set; }

        public int DeviceItemId { get; set; }
        public DeviceItem DeviceItem { get; set; }

        public string Description { get; set; } 
    }

public enum DeviceBorrowingStatus
{
    Pending,
    Approved,
    Rejected,
    Completed
}
