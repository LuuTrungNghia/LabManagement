namespace api.Dtos.User
{
    public class UpdateUserRequestDto
    {
        public required string Name { get; set; }
        public required string Role { get; set; }
        public required string Email { get; set; }
        public bool IsApproved { get; set; }
    }
}
