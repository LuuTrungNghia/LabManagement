namespace api.Dtos.Server
{
    public class CreateUserDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Password { get; set; }
    }
}
