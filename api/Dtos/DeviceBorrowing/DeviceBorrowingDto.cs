namespace api.Dtos.DeviceBorrowing
{
    public class DeviceBorrowingDto
    {
        public int Id { get; set; }
        public int DeviceId { get; set; }
        public int UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string BorrowerType { get; set; }
        public string DeviceStatus { get; set; }
        public bool IsApproved { get; set; }
    }
}