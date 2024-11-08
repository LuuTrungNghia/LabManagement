using api.Dtos.DeviceBorrowing;

namespace api.Dtos.LabBorrowing
{
    public class LabBorrowingDto
    {
        public int Id { get; set; }
        public string LabName { get; set; }
        public string BorrowerName { get; set; }
        public string BorrowerType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsApproved { get; set; }
        public List<DeviceBorrowingDto> Devices { get; set; }
    }
}