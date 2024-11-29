namespace api.Models
{
    public class ServerUser
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string UserServer { get; set; }
        public string PassServer { get; set; }
        public bool IsApproved { get; set; }
    }
}
