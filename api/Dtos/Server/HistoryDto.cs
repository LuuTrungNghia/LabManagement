namespace api.Dtos.Server
{
    public class HistoryDto
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string UserServer { get; set; }
        public bool IsApproved { get; set; }
    }
}
