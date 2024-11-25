namespace api.Dtos.User
{
    public class UpdateUserDto
    {
        public required string Email { get; set; }
        public string Fullname { get; set; }
        public string Avatar { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; } // "Male", "Female", "Other"
    }
}
