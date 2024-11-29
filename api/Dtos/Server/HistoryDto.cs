namespace api.Dtos.Server
{
    public class HistoryDto
    {
        public string Action { get; set; } // Hành động thực hiện
        public DateTime Timestamp { get; set; } // Thời gian thực hiện
        public string PerformedBy { get; set; } // Người thực hiện hành động
        public string TargetAccount { get; set; } // Tài khoản bị tác động
    }
}
