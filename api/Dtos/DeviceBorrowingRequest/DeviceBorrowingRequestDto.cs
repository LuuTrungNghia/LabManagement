namespace api.Dtos.DeviceBorrowingRequest
{
    public class DeviceBorrowingRequestDto
    {
        public int Id { get; set; }
        public int DeviceId { get; set; }
        public string DeviceName { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string Status { get; set; }
    }
}
