namespace api.Dtos.User
{
    public class RegisterDto
    {
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public string Role { get; set; }
        public bool IsValid()
        {
        return !string.IsNullOrEmpty(Role);
        }
    }
}
