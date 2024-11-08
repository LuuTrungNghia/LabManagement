namespace api.Models
{
   public class DeviceBorrowing
    {
        public int Id { get; set; }
        public int DeviceId { get; set; }
        public int UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string BorrowerType { get; set; }
        public DeviceStatus DeviceStatus { get; set; }
        public bool IsApproved { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}
