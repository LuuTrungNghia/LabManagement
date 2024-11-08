namespace api.Dtos.RoomBooking
{
    public class CreateRoomBookingRequestDto
    {
        public string RoomName { get; set; }
        public string Department { get; set; }
        public string BorrowerName { get; set; }
        public string BorrowerType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<int> DeviceIds { get; set; }
    }
}