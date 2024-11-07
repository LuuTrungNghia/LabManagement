namespace api.Models
{
    public class Lab
    {
         public int Id { get; set; }
        public string Name { get; set; }  // Tên phòng lab
        public string Description { get; set; }  // Mô tả phòng lab
        public string Location { get; set; }  // Vị trí phòng lab
        public bool IsAvailable { get; set; }  // Phòng còn sử dụng được không
        public string LabType { get; set; }  // Loại phòng lab (ví dụ: phòng thực hành, phòng lý thuyết)
        public List<LabBorrowingRequest> BorrowingRequests { get; set; }  // Danh sách yêu cầu mượn phòng lab cùng khoa
        public List<RoomBookingRequest> RoomBookingReques { get; set; }  // Danh sách yêu cầu mượn phòng lab khác khoa
    }
}
