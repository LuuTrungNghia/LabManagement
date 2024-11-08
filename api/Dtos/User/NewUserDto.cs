namespace api.Dtos.User
{
    public class NewUserDto
    {
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string Token { get; set; }
    }
}
