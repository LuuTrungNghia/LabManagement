namespace api.Dtos.DeviceBorrowing
{
    public class UpdateDeviceBorrowingRequestDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string BorrowerType { get; set; }
        public bool IsApproved { get; set; }
    }
}