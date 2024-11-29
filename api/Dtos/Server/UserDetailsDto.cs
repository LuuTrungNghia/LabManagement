namespace api.Dtos.Server
{
    public class UserDetailsDto
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public bool IsApproved { get; set; }
        public List<string> Roles { get; set; }
    }
}
