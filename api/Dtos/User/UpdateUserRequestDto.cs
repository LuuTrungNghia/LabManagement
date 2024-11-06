namespace api.Dtos.User
{
    public class UpdateUserRequestDto
    {
        public required string Name { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public bool IsApproved { get; set; }
    }
}
