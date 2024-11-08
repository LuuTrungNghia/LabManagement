namespace api.Models
{
    public class LabBorrowing
    {
        public int Id { get; set; }
        public int LabId { get; set; }
        public Lab Lab { get; set; }
        public string BorrowerName { get; set; }
        public string BorrowerType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsApproved { get; set; }
        public string Reason { get; set; }
        public List<DeviceBorrowing> DeviceBorrowings { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
