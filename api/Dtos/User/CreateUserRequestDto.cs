namespace api.Dtos.User
{
    public class CreateUserRequestDto
    {
        public required string Name { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
    }
}
