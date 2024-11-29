namespace api.Dtos.Server
{
    public class ServerUserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string UserServer { get; set; }
        public bool IsApproved { get; set; }
    }
}
